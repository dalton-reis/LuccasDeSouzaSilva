using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace POC_3D
{
    class MainWindow3 : GameWindow
    {
        int pgmID;
        int vsID;
        int fsID;

        int attribute_vcol;
        int attribute_vpos;
        int uniform_mview;

        int vbo_position;
        int vbo_color;
        int vbo_mview;
        int ibo_elements;

        Vector3[] vertdata;
        Vector3[] coldata;

        List<Volume> objects = new List<Volume>();

        int[] indicedata;

        float time = 0.0f;

        Camera cam = new Camera();

        public MainWindow3() : base(512, 512, new GraphicsMode(32, 24, 0, 4))
        {}

        void initProgram()
        {
            pgmID = GL.CreateProgram();

            LoadShader(@"resources\shaders\vs.glsl", ShaderType.VertexShader, pgmID, out vsID);
            LoadShader(@"resources\shaders\fs.glsl", ShaderType.FragmentShader, pgmID, out fsID);
            
            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));

            attribute_vpos = GL.GetAttribLocation(pgmID, "vPosition");
            attribute_vcol = GL.GetAttribLocation(pgmID, "vColor");
            uniform_mview = GL.GetUniformLocation(pgmID, "modelview");

            if (attribute_vpos == -1 || attribute_vcol == -1 || uniform_mview == -1)
            {
                Console.WriteLine("Error binding attributes");
            }

            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out vbo_mview);
            GL.GenBuffers(1, out ibo_elements);

            Random rand = new Random();

            for (int i = 0; i < 10; i++)
            {
                //                ColorCube c = new ColorCube(new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()));
                //                c.Position = new Vector3((float)rand.Next(-4, 4), (float)rand.Next(-4, 4), (float)rand.Next(-8, 8));
                //                c.Rotation = new Vector3((float)rand.Next(0, 6), (float)rand.Next(0, 6), (float)rand.Next(0, 6));
                //               c.Scale = Vector3.One * ((float)rand.NextDouble() + 0.2f);

                Sierpinski c = new Sierpinski((int)rand.Next(1,5));
                c.Position = new Vector3((float)rand.Next(-4, 4), (float)rand.Next(-4, 4), (float)rand.Next(-8, 8));
                c.Rotation = new Vector3((float)rand.Next(0, 6), (float)rand.Next(0, 6), (float)rand.Next(0, 6));
                c.Scale = Vector3.One * ((float)rand.NextDouble() + 0.2f);

                objects.Add(c);
            }

        }

        void LoadShader(string fileName, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);

            var src = File.ReadAllText(fileName);
            GL.ShaderSource(address, src);
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));


        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            initProgram();

            Title = "Hello OpenTK";

            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5.0f);
        }

        private void HandleKeyboard(float time)
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            if (keyState.IsKeyDown(Key.W))
            {
                cam.Move(0f, 0.1f, 0f, time);
            }
            else if (keyState.IsKeyDown(Key.A))
            {
                cam.Move(-0.1f, 0f, 0f, time);
            }
            else if (keyState.IsKeyDown(Key.S))
            {
                cam.Move(0f, -0.1f, 0f, time);
            }
            else if (keyState.IsKeyDown(Key.D))
            {
                cam.Move(0.1f, 0f, 0f, time);
            }
            else if (keyState.IsKeyDown(Key.Q))
            {
                cam.Move(0f, 0f, 0.1f, time);
            }
            else if (keyState.IsKeyDown(Key.E))
            {
                cam.Move(0f, 0f, -0.1f, time);
            }
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            time += (float) e.Time;

            HandleKeyboard((float) e.Time);

            List<Vector3> verts = new List<Vector3>();
            List<int> inds = new List<int>();
            List<Vector3> colors = new List<Vector3>();

            int vertcount = 0;

            foreach (Volume v in objects)
            {
                verts.AddRange(v.GetVerts().ToList());
                inds.AddRange(v.GetIndices(vertcount).ToList());
                colors.AddRange(v.GetColorData().ToList());
                vertcount += v.VertCount;
            }

            vertdata = verts.ToArray();
            indicedata = inds.ToArray();
            coldata = colors.ToArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vcol, 3, VertexAttribPointerType.Float, false, 0, 0);

            objects[0].Position = new Vector3(0.3f, -0.5f + (float)Math.Sin(time), -3.0f);
            objects[0].Scale = new Vector3(0.1f, 0.1f, 0.1f);

            objects[1].Position = new Vector3(-1f, 0.5f + (float)Math.Cos(time), -2.0f);
            objects[1].Scale = new Vector3(0.25f, 0.25f, 0.25f);

            foreach (Volume v in objects)
            {
                v.Rotation = new Vector3(0.55f * time, 0.25f * time, 0);

                v.CalculateModelMatrix();

                //v.ViewProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40.0f);

                v.ViewProjectionMatrix = cam.GetViewMatrix() 
                    * Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40.0f);

                v.ModelViewProjectionMatrix = v.ModelMatrix * v.ViewProjectionMatrix;
            }
            
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);

            GL.UseProgram(pgmID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            GL.EnableVertexAttribArray(attribute_vpos);
            GL.EnableVertexAttribArray(attribute_vcol);

            GL.DrawElements(PrimitiveType.Triangles, indicedata.Length, DrawElementsType.UnsignedInt, 0);

            int indiceat = 0;

            foreach (Volume v in objects)
            {
                GL.UniformMatrix4(uniform_mview, false, ref v.ModelViewProjectionMatrix);
                GL.DrawElements(PrimitiveType.Triangles, v.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += v.IndiceCount;
            }

            GL.DisableVertexAttribArray(attribute_vpos);
            GL.DisableVertexAttribArray(attribute_vcol);

            GL.Flush();

            SwapBuffers();
        }

    }
}
