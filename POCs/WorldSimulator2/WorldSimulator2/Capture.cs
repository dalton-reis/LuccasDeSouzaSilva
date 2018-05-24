/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/


using System;
using System.Runtime.InteropServices;

using System.Diagnostics;
using System.Windows.Forms;

using DirectShowLib;
using System.Drawing;
using System.Threading;

namespace WorldSimulator
{
    /// <summary> Summary description for MainForm. </summary>
    internal class Capture : ISampleGrabberCB, IDisposable
    {
        #region Member variables

        /// <summary> graph builder interface. </summary>
		private IFilterGraph2 m_FilterGraph = null;
        IMediaControl m_mediaCtrl = null;

        // Used to snap picture on Still pin
        private IAMVideoControl m_VidControl = null;
        private IPin m_pinStill = null;

        /// <summary> Dimensions of the image, calculated once in constructor for perf. </summary>
        private int m_videoWidth;
        private int m_videoHeight;
        private int m_stride;

        /// <summary> so we can wait for the async job to finish </summary>
        private ManualResetEvent m_PictureReady = null;

        private bool m_WantOne = false;

        /// <summary> Set by async routine when it captures an image </summary>
        private bool m_bRunning = false;

        Form windowForm = null;

        private IntPtr m_ipBuffer = IntPtr.Zero;

#if DEBUG
        DsROTEntry m_rot = null;
#endif

        #endregion

        /// <summary> release everything. </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            CloseInterfaces();
        }

        ~Capture()
        {
            Dispose();
        }

        #region APIs
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);
        #endregion

        /// <summary>
        /// Create capture object
        /// </summary>
        /// <param name="iDeviceNum">Zero based index of capture device</param>
        /// <param name="szFileName">Output ASF file name</param>
        /// int iDeviceNum, int iWidth, int iHeight, short iBPP, Control hControl
        public Capture(int[] iVideoConfig, Control hControl, string szOutputFileName)
        {
            int iDeviceNum = iVideoConfig[0];
            int iWidth = iVideoConfig[1];
            int iHeight = iVideoConfig[2];
            short iBPP = (short) iVideoConfig[3];
            
            DsDevice[] capDevices;

            // Get the collection of video devices
            capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            try
            {
                // Set up the capture graph
                SetupGraph(capDevices[iDeviceNum],  iWidth, iHeight, iBPP, hControl, szOutputFileName);

                m_bRunning = false;
            }
            catch
            {
                Dispose();
                throw;
            }
        }


        // Start the capture graph
        public void Start()
        {
            if (!m_bRunning)
            {
                int hr = m_mediaCtrl.Run();
                Marshal.ThrowExceptionForHR(hr);

                m_bRunning = true;
            }
        }

        // Pause the capture graph.
        // Running the graph takes up a lot of resources.  Pause it when it
        // isn't needed.
        public void Pause()
        {
            if (m_bRunning)
            {
                IMediaControl mediaCtrl = m_FilterGraph as IMediaControl;

                int hr = mediaCtrl.Pause();
                Marshal.ThrowExceptionForHR(hr);

                m_bRunning = false;
            }
        }

        /// <summary> build the capture graph. </summary>
        private void SetupGraph(DsDevice dev, int iWidth, int iHeight, short iBPP, Control hControl, string szOutputFileName)
        {
            int hr;

            ISampleGrabber sampGrabber = null;
            IPin pCaptureOut = null;
            IPin pSampleIn = null;
            IPin pRenderIn = null;

            IBaseFilter capFilter = null;
            IBaseFilter asfWriter = null;
            ICaptureGraphBuilder2 capGraph = null;

            // Get the graphbuilder object
            m_FilterGraph = (IFilterGraph2) new FilterGraph();

            IBaseFilter iSmartTee = null;


            try
            {
#if DEBUG
                m_rot = new DsROTEntry(m_FilterGraph);
#endif
                //CREATE FILESAVE
                // Get the ICaptureGraphBuilder2
                capGraph = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();

                ISampleGrabber samplegrabber = (ISampleGrabber) new SampleGrabber();
                IBaseFilter basegrab = (IBaseFilter) samplegrabber;

                AMMediaType media;
                media = new AMMediaType();
                media.majorType = MediaType.Video;
                media.subType = MediaSubType.ARGB32;
                media.formatType = FormatType.VideoInfo2;
                samplegrabber.SetMediaType(media);

                DsUtils.FreeAMMediaType(media);
                samplegrabber.SetBufferSamples(true);

                m_FilterGraph.AddFilter(basegrab, "filter");

                // Start building the graph
                hr = capGraph.SetFiltergraph(m_FilterGraph);
                Marshal.ThrowExceptionForHR(hr);

                // Add the capture device to the graph
                hr = m_FilterGraph.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out capFilter);
                Marshal.ThrowExceptionForHR(hr);

                //CREATE PREVIEW
                // Didn't find one.  Is there a preview pin?
                if (m_pinStill == null)
                {
                    m_pinStill = DsFindPin.ByCategory(capFilter, PinCategory.Preview, 0);
                }

                // Still haven't found one.  Need to put a splitter in so we have
                // one stream to capture the bitmap from, and one to display.  Ok, we
                // don't *have* to do it that way, but we are going to anyway.
                if (m_pinStill == null)
                {
                    IPin pRaw = null;
                    IPin pSmart = null;

                    // There is no still pin
                    m_VidControl = null;

                    // Add a splitter
                    iSmartTee = (IBaseFilter)new SmartTee();

                    try
                    {
                        hr = m_FilterGraph.AddFilter(iSmartTee, "SmartTee");
                        DsError.ThrowExceptionForHR(hr);

                        // Find the find the capture pin from the video device and the
                        // input pin for the splitter, and connnect them
                        pRaw = DsFindPin.ByCategory(capFilter, PinCategory.Capture, 0);
                        pSmart = DsFindPin.ByDirection(iSmartTee, PinDirection.Input, 0);

                        hr = m_FilterGraph.Connect(pRaw, pSmart);
                        DsError.ThrowExceptionForHR(hr);

                        // Now set the capture and still pins (from the splitter)
                        m_pinStill = DsFindPin.ByName(iSmartTee, "Preview");
                        pCaptureOut = DsFindPin.ByName(iSmartTee, "Capture");

                        // If any of the default config items are set, perform the config
                        // on the actual video device (rather than the splitter)
                        if (iHeight + iWidth + iBPP > 0)
                        {
                            SetConfigParms(pRaw, iWidth, iHeight, iBPP);
                        }
                    }
                    finally
                    {
                        if (pRaw != null)
                        {
                            Marshal.ReleaseComObject(pRaw);
                        }
                        if (pRaw != pSmart)
                        {
                            Marshal.ReleaseComObject(pSmart);
                        }
                        if (pRaw != iSmartTee)
                        {
                            Marshal.ReleaseComObject(iSmartTee);
                        }
                    }
                }
                else
                {
                    // Get a control pointer (used in Click())
                    m_VidControl = capFilter as IAMVideoControl;

                    pCaptureOut = DsFindPin.ByCategory(capFilter, PinCategory.Capture, 0);

                    // If any of the default config items are set
                    if (iHeight + iWidth + iBPP > 0)
                    {
                        SetConfigParms(m_pinStill, iWidth, iHeight, iBPP);
                    }
                }

                // Get the SampleGrabber interface
                sampGrabber = new SampleGrabber() as ISampleGrabber;

                // Configure the sample grabber
                IBaseFilter baseGrabFlt = sampGrabber as IBaseFilter;
                ConfigureSampleGrabber(sampGrabber);
                pSampleIn = DsFindPin.ByDirection(baseGrabFlt, PinDirection.Input, 0);

                // Get the default video renderer
                IBaseFilter pRenderer = new VideoRendererDefault() as IBaseFilter;
                hr = m_FilterGraph.AddFilter(pRenderer, "Renderer");
                DsError.ThrowExceptionForHR(hr);

                pRenderIn = DsFindPin.ByDirection(pRenderer, PinDirection.Input, 0);

                // Add the sample grabber to the graph
                hr = m_FilterGraph.AddFilter(baseGrabFlt, "Ds.NET Grabber");
                DsError.ThrowExceptionForHR(hr);

                if (m_VidControl == null)
                {
                    // Connect the Still pin to the sample grabber
                    hr = m_FilterGraph.Connect(m_pinStill, pSampleIn);
                    DsError.ThrowExceptionForHR(hr);

                    // Connect the capture pin to the renderer
                    hr = m_FilterGraph.Connect(pCaptureOut, pRenderIn);
                    DsError.ThrowExceptionForHR(hr);
                }
                else
                {
                    // Connect the capture pin to the renderer
                    hr = m_FilterGraph.Connect(pCaptureOut, pRenderIn);
                    DsError.ThrowExceptionForHR(hr);

                    // Connect the Still pin to the sample grabber
                    hr = m_FilterGraph.Connect(m_pinStill, pSampleIn);
                    DsError.ThrowExceptionForHR(hr);
                }

                // Learn the video properties
                SaveSizeInfo(sampGrabber);
                ConfigVideoWindow(hControl);

                //END PREVIEW   
                asfWriter = ConfigAsf(capGraph, szOutputFileName);

                hr = capGraph.RenderStream(null, null, capFilter, null, asfWriter);
                Marshal.ThrowExceptionForHR(hr);

                m_mediaCtrl = m_FilterGraph as IMediaControl;

                // Start the graph
                hr = m_mediaCtrl.Run();
                DsError.ThrowExceptionForHR(hr);
            }
            finally
            {
                if (capFilter != null)
                {
                    Marshal.ReleaseComObject(capFilter);
                    capFilter = null;
                }
                if (asfWriter != null)
                {
                    Marshal.ReleaseComObject(asfWriter);
                    asfWriter = null;
                }
                if (capGraph != null)
                {
                    Marshal.ReleaseComObject(capGraph);
                    capGraph = null;
                }
            }
        }

        // Set the Framerate, and video size
        private void SetConfigParms(IPin pStill, int iWidth, int iHeight, short iBPP)
        {
            int hr;
            AMMediaType media;
            VideoInfoHeader v;

            IAMStreamConfig videoStreamConfig = pStill as IAMStreamConfig;

            // Get the existing format block
            hr = videoStreamConfig.GetFormat(out media);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                // copy out the videoinfoheader
                v = new VideoInfoHeader();
                Marshal.PtrToStructure(media.formatPtr, v);

                // if overriding the width, set the width
                if (iWidth > 0)
                {
                    v.BmiHeader.Width = iWidth;
                }

                // if overriding the Height, set the Height
                if (iHeight > 0)
                {
                    v.BmiHeader.Height = iHeight;
                }

                // if overriding the bits per pixel
                if (iBPP > 0)
                {
                    v.BmiHeader.BitCount = iBPP;
                }

                // Copy the media structure back
                Marshal.StructureToPtr(v, media.formatPtr, false);

                // Set the new format
                hr = videoStreamConfig.SetFormat(media);
                //DsError.ThrowExceptionForHR( hr );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                DsUtils.FreeAMMediaType(media);
                media = null;
            }
        }

        private void SaveSizeInfo(ISampleGrabber sampGrabber)
        {
            int hr;

            // Get the media type from the SampleGrabber
            AMMediaType media = new AMMediaType();

            hr = sampGrabber.GetConnectedMediaType(media);
            DsError.ThrowExceptionForHR(hr);

            if ((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero))
            {
                throw new NotSupportedException("Unknown Grabber Media Format");
            }

            // Grab the size info
            VideoInfoHeader videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
            m_videoWidth = videoInfoHeader.BmiHeader.Width;
            m_videoHeight = videoInfoHeader.BmiHeader.Height;
            m_stride = m_videoWidth * (videoInfoHeader.BmiHeader.BitCount / 8);

            DsUtils.FreeAMMediaType(media);
            media = null;
        }

        // Set the video window within the control specified by hControl
        private void ConfigVideoWindow(Control hControl)
        {
            int hr;

            IVideoWindow ivw = m_FilterGraph as IVideoWindow;

            // Set the parent
            hr = ivw.put_Owner(hControl.Handle);
            DsError.ThrowExceptionForHR(hr);

            // Turn off captions, etc
            hr = ivw.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren | WindowStyle.ClipSiblings);
            DsError.ThrowExceptionForHR(hr);

            // Yes, make it visible
            hr = ivw.put_Visible(OABool.True);
            DsError.ThrowExceptionForHR(hr);

            // Move to upper left corner
            Rectangle rc = hControl.ClientRectangle;
            hr = ivw.SetWindowPosition(0, 0, rc.Right, rc.Bottom);
            DsError.ThrowExceptionForHR(hr);
        }

        private void ConfigureSampleGrabber(ISampleGrabber sampGrabber)
        {
            int hr;
            AMMediaType media = new AMMediaType();

            // Set the media type to Video/RBG24
            media.majorType = MediaType.Video;
            media.subType = MediaSubType.RGB24;
            media.formatType = FormatType.VideoInfo;
            hr = sampGrabber.SetMediaType(media);
            DsError.ThrowExceptionForHR(hr);

            DsUtils.FreeAMMediaType(media);
            media = null;

            // Configure the samplegrabber
            hr = sampGrabber.SetCallback(this, 1);
            DsError.ThrowExceptionForHR(hr);
        }

        private IBaseFilter ConfigAsf(ICaptureGraphBuilder2 capGraph, string szOutputFileName)
        {
            IFileSinkFilter pTmpSink = null;
            IBaseFilter asfWriter = null;

            int hr = capGraph.SetOutputFileName(MediaSubType.Asf, szOutputFileName, out asfWriter, out pTmpSink);
            Marshal.ThrowExceptionForHR(hr);

            try
            {
                IConfigAsfWriter lConfig = asfWriter as IConfigAsfWriter;

                // Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)
                // READ THE README for info about using guids
                Guid cat = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);
                
                hr = lConfig.ConfigureFilterUsingProfileGuid(cat);
                Marshal.ThrowExceptionForHR(hr);
            }
            finally
            {
                Marshal.ReleaseComObject(pTmpSink);
            }

            return asfWriter;
        }

        /// <summary> Shut down capture </summary>
		private void CloseInterfaces()
        {
            int hr;

            try
            {
                if (m_mediaCtrl != null)
                {
                    // Stop the graph
                    hr = m_mediaCtrl.Stop();
                    m_bRunning = false;
                }
            }
            catch
            {
            }

#if DEBUG
            // Remove graph from the ROT
            if (m_rot != null)
            {
                m_rot.Dispose();
                m_rot = null;
            }
#endif

            if (m_FilterGraph != null)
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                m_FilterGraph = null;
            }
        }

        /// <summary> sample callback, NOT USED. </summary>
        int ISampleGrabberCB.SampleCB(double SampleTime, IMediaSample pSample)
        {
            Marshal.ReleaseComObject(pSample);
            return 0;
        }

        /// <summary> buffer callback, COULD BE FROM FOREIGN THREAD. </summary>
        int ISampleGrabberCB.BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
        {
            // Note that we depend on only being called once per call to Click.  Otherwise
            // a second call can overwrite the previous image.
            Debug.Assert(BufferLen == Math.Abs(m_stride) * m_videoHeight, "Incorrect buffer length");

            if (m_WantOne)
            {
                m_WantOne = false;
                Debug.Assert(m_ipBuffer != IntPtr.Zero, "Unitialized buffer");

                // Save the buffer
                CopyMemory(m_ipBuffer, pBuffer, BufferLen);

                // Picture is ready.
                m_PictureReady.Set();
            }

            return 0;
        }
    }
}
