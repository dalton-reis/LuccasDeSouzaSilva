package GUI;

import java.awt.BorderLayout;

import javax.media.opengl.GLCanvas;
import javax.media.opengl.GLCapabilities;
import javax.swing.JFrame;
import javax.swing.WindowConstants;

public class Camera extends JFrame {

	private static final long serialVersionUID = 1L;
	private Mundo mundo = new Mundo(this);
	private GLCanvas canvasScreen;

	private int janelaLargura = 600, janelaAltura = 600;

	/**
	 * Método que define o canvas do projeto
	 */
	public Camera() {

		// Cria o frame.
		super("CG-N4 (Luccas Silva e Lucas Schlogl");
		setBounds(300, 150, janelaLargura + 16, janelaAltura + 38);
		// 400 + 22 da borda do titulo da janela

		setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);
		getContentPane().setLayout(new BorderLayout());

		/*
		 * Cria um objeto GLCapabilities para especificar o numero de bits por
		 * pixel para RGBA
		 */
		GLCapabilities glCaps = new GLCapabilities();
		glCaps.setRedBits(8);
		glCaps.setBlueBits(8);
		glCaps.setGreenBits(8);
		glCaps.setAlphaBits(8);

		canvasScreen = new GLCanvas(glCaps);
		add(canvasScreen);
		canvasScreen.addGLEventListener(mundo);
		canvasScreen.addMouseListener(mundo);
		canvasScreen.addMouseMotionListener(mundo);
		canvasScreen.addKeyListener(mundo);
		canvasScreen.setBounds(22, 22, 534, 256);

	}

	/**
	 * Método main do projeto
	 * 
	 * @param args
	 *            - parametros passados via inicializaçao
	 */
	public static void main(String[] args) {
		new Camera().setVisible(true);
	}

	/**
	 * Retorna o screenCanvas
	 */
	public Mundo getMundo() {
		return mundo;
	}

}
