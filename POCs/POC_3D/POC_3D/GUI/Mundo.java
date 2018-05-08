package GUI;

import Default.Bloco;
import Default.Mapa;
import Default.Object3D;
import Default.Ponto4D;

import com.sun.opengl.util.GLUT;
import com.sun.opengl.util.texture.TextureData;

import java.awt.Component;
import java.awt.PopupMenu;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.awt.event.MouseWheelEvent;
import java.awt.event.MouseWheelListener;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

import javax.imageio.ImageIO;
import javax.media.opengl.DebugGL;
import javax.media.opengl.GL;
import javax.media.opengl.GLAutoDrawable;
import javax.media.opengl.GLEventListener;
import javax.media.opengl.glu.GLU;
import javax.swing.JOptionPane;

import object.OBJModel;

public class Mundo implements GLEventListener, KeyListener, MouseWheelListener, MouseListener, MouseMotionListener {

    private BufferedImage image;
    private int widthImg, heightImg;
    private TextureData td;
    private ByteBuffer buffer[];
    private int idTexture[];

    // define as constantes
    public final int PERSPECTIVE = 0;
    public final int LOOKAT = 3;

    // define as variáveis
    protected GL gl;
    protected GLU glu;
    protected GLUT glut;
    protected GLAutoDrawable glDrawable;

    // armazena um ponteiro para o frame principal
    protected Camera frame;

    // informa as dimensões da tela
    protected int heigth = 256;
    protected int width = 256;
    protected int x = 0;
    protected int y = 0;

    // informa se a tela foi redimensionada
    protected boolean redim = false;

    // objetos3D de cena
    static private Mapa mapa = null;

    // objeto3D de cena
    static private Object3D safeZone = null;

    // objeto3D carro
    static private Object3D objCarro = null;
    private double angHor = 0;
    private double acel = 0.25;
    boolean boolAcel = false;
    int keyAcel = 0;
    // cima, baixo, direita, esquerda
    static List<Integer> keys = new ArrayList<Integer>();

    // informa o valor y do último pressionamento do botão esquerdo do mouse
    private int old_y; // translacao camera
    private int old_x; // translacao camera

    // define os vetores de commando
    //
    protected float[] lookat = {0.0f, 12.0f, -4.0f, // eye x, y, z
        0.0f, 0.0f, 0.0f, // center x, y, z
        0.0f, 1.0f, 0.0f}; // up vector x, y, z

    protected float[] perspective = {90.0f, 1.5f, 0.1f, 100.0f}; //
    // field of view angle(degrees) in y direction
    // field of view x direction (width/height)
    // distance near
    // distance far

    protected float[] light = {1.5f, 1.0f, 1.0f, 0.0f}; // x, y, z

    // , directional (0) / positional (1)
    public Mundo(Camera frame) {
        this.frame = frame;
    }

    public void init(GLAutoDrawable drawable) {
        glDrawable = drawable;
        gl = drawable.getGL();
        glu = new GLU();
        glut = new GLUT();
        glDrawable.setGL(new DebugGL(gl));

        reshape(glDrawable, 0, 0, 534, 256);

        gl.glClearColor(1.0f, 1.0f, 1.0f, 1.0f);
        gl.glEnable(GL.GL_DEPTH_TEST);
        gl.glShadeModel(GL.GL_SMOOTH);

        escolheMapa();

        // sempre inicializa na pos 0.0, 0.0, 0.0
        objCarro = new Object3D(gl, new OBJModel("data/porsche", 3.0f, gl, true), mapa.getCarPos(), new double[]{1.4,
            1.0, 3.0});

        Thread th = new Thread(new Runnable() {
            @Override
            public void run() {
                while (true) {
                    if (!keys.isEmpty()) {
                        for (Integer key : keys) {
                            // int escala = acel;
                            switch (key) {
                                case KeyEvent.VK_UP:
                                    // para objeto carro

                                    objCarro.rotacaoY(angHor);
                                    objCarro.translacaoXYZ(0.0, 0.0, acel);
                                    // lookat[3] = objCarro.getPosX();
                                    // lookat[4] = objCarro.getPosY();
                                    // lookat[5] = objCarro.getPosZ();
                                    //
                                    // System.out.println("[" + objCarro.getPosX() +
                                    // "," + objCarro.getPosY() + ","
                                    // + objCarro.getPosZ() + "]");
                                    break;
                                case KeyEvent.VK_DOWN:
                                    // para objeto carro

                                    objCarro.rotacaoY(-angHor);
                                    objCarro.translacaoXYZ(0.0, 0.0, -acel);

                                    // lookat[3] = objCarro.getPosX();
                                    // lookat[4] = objCarro.getPosY();
                                    // lookat[5] = objCarro.getPosZ();
                                    break;
                                case KeyEvent.VK_LEFT:
                                    if (angHor < 10) {
                                        angHor += 0.5;
                                    }
                                    // System.out.println(angHor);
                                    break;
                                case KeyEvent.VK_RIGHT:
                                    if (angHor > -10) {
                                        angHor -= 0.5;
                                    }
                                    // System.out.println(angHor);
                                    break;
                            }
                        }

                        // atualiza o screen windows e world windows
                        frame.getMundo().getGLDrawable().display();
                    }
                    try {
                        Thread.sleep(50);
                    } catch (InterruptedException e) {
                        // TODO Auto-generated catch block
                        e.printStackTrace();
                    }
                }
            }
        });

        th.start();

        ligarLuz();

        gl.glPushMatrix();
        // ===== Comandos de inicializacao para textura
        idTexture = new int[3]; // lista de identificadores de textura
        gl.glGenTextures(1, idTexture, 2); // Gera identificador de textura
        // Define os filtros de magnificacao e minificacao
        gl.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_LINEAR);
        gl.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_LINEAR);
        buffer = new ByteBuffer[2]; // buffer da imagem
        loadImage(0, "data/cone2.jpg"); // carrega as texturas
        gl.glPopMatrix();

    }

    public void display(GLAutoDrawable arg0) {

        boolean colisao = calculaColisao();
        if (colisao) {
            JOptionPane.showMessageDialog(null, "Voce bateu. Jogo encerrado.");
            System.exit(0);
        }

        float aux[] = perspective, look[] = lookat;

        gl.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);
        gl.glMatrixMode(GL.GL_PROJECTION);
        gl.glLoadIdentity();

        // define visão da camera
        aux = getCellArray(PERSPECTIVE);
        look = getCellArray(LOOKAT);

        glu.gluPerspective(aux[0], aux[1], aux[2], aux[3]);

        gl.glMatrixMode(GL.GL_MODELVIEW);
        gl.glLoadIdentity();

        lookat = getCellArray(LOOKAT);

        // atualiza visão da camera
        try {
            float[] direcao = objCarro.getTranslation();

            glu.gluLookAt(mapa.getCamPos()[0], mapa.getCamPos()[1], mapa.getCamPos()[2], // eye
                    direcao[0], direcao[1], direcao[2], // center
                    lookat[6], lookat[7], lookat[8]); // up vector
        } catch (Exception e) {
        }

        drawAxis();

        gl.glEnable(GL.GL_TEXTURE_2D); // Primeiro habilita uso de textura
        gl.glBindTexture(GL.GL_TEXTURE_2D, idTexture[0]); // Especifica qual e a
        // textura corrente
        // pelo
        // identificador
        gl.glTexImage2D(GL.GL_TEXTURE_2D, 0, 3, widthImg, heightImg, 0, GL.GL_BGR, GL.GL_UNSIGNED_BYTE, buffer[0]); // Envio
        // da
        // textura
        // para
        // OpenGL
        mapa.desenhaMapa();
        gl.glDisable(GL.GL_TEXTURE_2D); // Desabilita uso de textura

        objCarro.desenha();

        // calcula colisao
        gl.glFlush();
    }

    public void drawAxis() {
        // eixo X - Red
        gl.glColor3f(1.0f, 0.0f, 0.0f);
        gl.glBegin(GL.GL_LINES);
        gl.glVertex3f(-20.0f, 0.0f, 0.0f);
        gl.glVertex3f(20.0f, 0.0f, 0.0f);
        gl.glEnd();
        // eixo Y - Green
        gl.glColor3f(0.0f, 1.0f, 0.0f);
        gl.glBegin(GL.GL_LINES);
        gl.glVertex3f(0.0f, 0.0f, 0.0f);
        gl.glVertex3f(0.0f, 40.0f, 0.0f);
        gl.glEnd();
        // eixo Z - Blue
        gl.glColor3f(0.0f, 0.0f, 1.0f);
        gl.glBegin(GL.GL_LINES);
        gl.glVertex3f(0.0f, 0.0f, -20.0f);
        gl.glVertex3f(0.0f, 0.0f, 20.0f);
        gl.glEnd();
    }

    public void reshape(GLAutoDrawable drawable, int x, int y, int width, int height) {

        int subWidth = width;
        int subHeight = height;

        if (gl != null) {
            gl.glViewport(0, 0, subWidth, subHeight);
            gl.glMatrixMode(GL.GL_PROJECTION);
            gl.glLoadIdentity();

            glu.gluPerspective(perspective[0], perspective[1], perspective[2], perspective[3]);

            gl.glGetDoublev(GL.GL_PROJECTION_MATRIX, new double[16], 1);
            gl.glMatrixMode(GL.GL_MODELVIEW);
            gl.glLoadIdentity();

            // atualiza visão da camera
            glu.gluLookAt(lookat[0], lookat[1], lookat[2], lookat[3], lookat[4], lookat[5], lookat[6], lookat[7],
                    lookat[8]);

            gl.glGetDoublev(GL.GL_MODELVIEW_MATRIX, new double[16], 1);
            gl.glClearColor(0.2f, 0.2f, 0.2f, 0.0f);
            gl.glEnable(GL.GL_DEPTH_TEST);
            gl.glEnable(GL.GL_LIGHTING);
            gl.glEnable(GL.GL_LIGHT0);
        }
    }

    public void displayChanged(GLAutoDrawable arg0, boolean arg1, boolean arg2) {
        // throw new UnsupportedOperationException("Not supported yet.");
    }

    public void keyTyped(KeyEvent e) {
        // throw new UnsupportedOperationException("Not supported yet.");
    }

    public void keyPressed(KeyEvent e) {
        switch (e.getKeyCode()) {

            case KeyEvent.VK_R:
                System.out.println("Tecla R!");
                if (!keys.contains(KeyEvent.VK_R)) {
                    keys.add(KeyEvent.VK_R);
                }
                break;
            case KeyEvent.VK_UP:
                if (!keys.contains(KeyEvent.VK_UP)) {
                    keys.add(KeyEvent.VK_UP);
                    if (keys.contains(KeyEvent.VK_DOWN)) {
                        keys.remove(Integer.valueOf(KeyEvent.VK_DOWN));
                    }
                }
                break;
            case KeyEvent.VK_DOWN:
                if (!keys.contains(KeyEvent.VK_DOWN)) {
                    keys.add(KeyEvent.VK_DOWN);
                    if (keys.contains(KeyEvent.VK_UP)) {
                        keys.remove(Integer.valueOf(KeyEvent.VK_UP));
                    }
                }
                break;
            case KeyEvent.VK_LEFT:
                // System.out.println(angHor);
                if (!keys.contains(KeyEvent.VK_LEFT)) {
                    keys.add(KeyEvent.VK_LEFT);
                    if (keys.contains(KeyEvent.VK_RIGHT)) {
                        keys.remove(Integer.valueOf(KeyEvent.VK_RIGHT));
                    }
                }
                break;
            case KeyEvent.VK_RIGHT:
                // System.out.println(angHor);
                if (!keys.contains(KeyEvent.VK_RIGHT)) {
                    keys.add(KeyEvent.VK_RIGHT);
                    if (keys.contains(KeyEvent.VK_LEFT)) {
                        keys.remove(Integer.valueOf(KeyEvent.VK_LEFT));
                    }
                }
                break;
            case KeyEvent.VK_W:
                // lookat[0]; // eye x, selec 1
                // lookat[1]; // eye y, selec 2
                lookat[2] += 2; // eye z, selec 3
                break;
            case KeyEvent.VK_A:
                lookat[0] += 2; // eye x, selec 1
                // lookat[1]; // eye y, selec 2
                // lookat[2]; // eye z, selec 3
                break;
            case KeyEvent.VK_S:
                // lookat[0]; // eye x, selec 1
                // lookat[1]; // eye y, selec 2
                lookat[2] += -2; // eye z, selec 3
                break;
            case KeyEvent.VK_D:
                lookat[0] += -2; // eye x, selec 1
                // lookat[1]; // eye y, selec 2
                // lookat[2]; // eye z, selec 3
                break;
            case KeyEvent.VK_SPACE:
                // lookat[0]; // eye x, selec 1
                lookat[1] += 0.5; // eye y, selec 2
                // lookat[2]; // eye z, selec 3
                break;
            case KeyEvent.VK_CONTROL:
                // lookat[0]; // eye x, selec 1
                lookat[1] += -0.5; // eye y, selec 2
                // lookat[2]; // eye z, selec 3
                break;

        }

        frame.getMundo().getGLDrawable().display();
    }

    public void keyReleased(KeyEvent e) {

        switch (e.getKeyCode()) {

            case KeyEvent.VK_R:
                if (keys.contains(KeyEvent.VK_R)) {
                    keys.remove(Integer.valueOf(KeyEvent.VK_R));
                }
                break;
            case KeyEvent.VK_UP:
                if (keys.contains(KeyEvent.VK_UP)) {
                    keys.remove(Integer.valueOf(KeyEvent.VK_UP));
                }
                break;
            case KeyEvent.VK_DOWN:
                if (keys.contains(KeyEvent.VK_DOWN)) {
                    keys.remove(Integer.valueOf(KeyEvent.VK_DOWN));
                }
                break;
            case KeyEvent.VK_LEFT:
                if (keys.contains(KeyEvent.VK_LEFT)) {
                    keys.remove(Integer.valueOf(KeyEvent.VK_LEFT));
                }
                break;
            case KeyEvent.VK_RIGHT:
                if (keys.contains(KeyEvent.VK_RIGHT)) {
                    keys.remove(Integer.valueOf(KeyEvent.VK_RIGHT));
                }
                break;
        }
    }

    public void mouseWheelMoved(MouseWheelEvent e) {
    }

    public void mouseClicked(MouseEvent e) {

    }

    public void mousePressed(MouseEvent e) {
    }

    public void mouseReleased(MouseEvent e) {
        // throw new UnsupportedOperationException("Not supported yet.");
    }

    public void mouseEntered(MouseEvent e) {
        // throw new UnsupportedOperationException("Not supported yet.");
    }

    public void mouseExited(MouseEvent e) {
        // throw new UnsupportedOperationException("Not supported yet.");
    }

    public void mouseDragged(MouseEvent e) {
    }

    public void mouseMoved(MouseEvent e) {
        // throw new UnsupportedOperationException("Not supported yet.");
    }

    /**
     * Retorna glDrawable.
     */
    public GLAutoDrawable getGLDrawable() {
        return glDrawable;
    }

    /**
     * Ativa display dos canvas.
     *
     * public void activeDisplay() { displayAll = true;
     *
     * glDrawable.display(); }
     */
    /**
     * Atribui mensagem de redimensionamento da tela.
     */
    public void redimensionar(boolean first) {
        if (!first) {
            redim = true;

            reshape(glDrawable, x, y, width, heigth);
            if (glDrawable != null) {
                glDrawable.display();
            }
        }
    }

    public float[] getCellArray(int tipo) {
        if (tipo == PERSPECTIVE) {
            return perspective;
        } else // (tipo == LOOKAT)
        {
            return lookat;
        }
    }

    private void ligarLuz() {
        float posLight[] = {5.0f, 5.0f, 10.0f, 1.0f};
        gl.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, posLight, 0);
        gl.glEnable(GL.GL_LIGHT0);
    }

    public void escolheMapa() {
        // float corRed[] = { 1.0f, 0.0f, 0.0f, 1.0f };
        // float corGreen[] = { 0.0f, 1.0f, 0.0f, 1.0f };
        // float corBlue[] = { 0.0f, 0.0f, 1.0f, 1.0f };
        // float corWhite[] = { 1.0f, 1.0f, 1.0f, 1.0f };
        // float corBlack[] = { 0.0f, 0.0f, 0.0f, 1.0f };

        char[][] field = { // 8x7 -- S (Safe), C (Car (aponta pra direita))
            //            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, // -------------------
            //            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, // -------------------
            //            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, // -------------------
            //            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, // -------------------
            //            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, // -------------------
            //            {' ', ' ', ' ', ' ', ' ', ' ', ' ', '=', '=', '='}, // -------------------
            //            {' ', ' ', ' ', ' ', ' ', ' ', ' ', '=', 'S', '='}, // -------------------
            //            {'=', '=', '=', '=', '=', '=', '=', '=', ' ', ' '}, // -------------------
            //            {'=', '=', '=', '=', ' ', '=', '=', '=', 'C', ' '}, // -------------------
            //            {'=', '=', '=', '=', '=', '=', '=', '=', '=', '='}}; // -----------------

            {' ', ' ', '=', '=', '=', '=', ' ', ' ', ' ', ' '}, // -------------------
            {' ', ' ', '=', ' ', ' ', '=', ' ', ' ', ' ', ' '}, // -------------------
            {' ', ' ', '=', '=', ' ', ' ', '=', ' ', ' ', ' '}, // -------------------
            {' ', ' ', '=', ' ', ' ', ' ', '=', ' ', ' ', ' '}, // -------------------
            {' ', ' ', '=', 'C', ' ', '=', ' ', ' ', ' ', ' '}, // -------------------
            {' ', ' ', '=', ' ', ' ', '=', ' ', '=', '=', ' '}, // -------------------
            {' ', ' ', '=', ' ', ' ', ' ', ' ', '=', ' ', ' '}, // -------------------
            {' ', ' ', '=', '=', '=', '=', '=', '=', ' ', '='}, // -------------------
            {' ', ' ', '=', '=', ' ', '=', '=', '=', ' ', 'S'}, // -------------------
            {' ', ' ', '=', '=', '=', '=', '=', '=', '=', '='}}; // -----------------

        HashMap<Character, OBJModel> map = new HashMap<>();
        map.put('=', new OBJModel("data/cone2_obj", 0.5f, gl, true));
        map.put('C', null); // automatico
        map.put('S', null); // automatico
        map.put(' ', null);

        mapa = new Mapa(gl, field, 2.4, map);

    }

    public void loadImage(int ind, String fileName) {
        // Tenta carregar o arquivo
        image = null;
        try {
            image = ImageIO.read(new File(fileName));
        } catch (IOException e) {
            JOptionPane.showMessageDialog(null, "Erro na leitura do arquivo " + fileName);
        }

        // Obtem largura e altura
        widthImg = image.getWidth();
        heightImg = image.getHeight();
        // Gera uma nova TextureData...
        td = new TextureData(0, 0, false, image);
        // ...e obtem um ByteBuffer a partir dela
        buffer[ind] = (ByteBuffer) td.getBuffer();
    }

    public boolean calculaColisao() {
        for (Object3D obj : mapa.getObjetos()) {
            if (dist(obj.getBboxCenter(), objCarro.getBboxCenter()) + 0.5 < 1.5 && !obj.ehSafety) {
//                System.out.println(dist(obj.getBboxCenter(), objCarro.getBboxCenter()) + 0.5);
                return true;
            }
        }
        Ponto4D pSafety = new Ponto4D(mapa.getSafePos()[0], 1, mapa.getSafePos()[2], 1);
        if (dist(pSafety, objCarro.getBboxCenter()) < 0.25) {
            JOptionPane.showMessageDialog(null, "Voce estacionou no local indicado. Parabens!");
            System.exit(0);
        }
        return false;
    }

    public double dist(Ponto4D p1, Ponto4D p2) {
        return (Math.pow(p1.obterX() - p2.obterX(), 2) + Math.pow(p1.obterZ() - p2.obterZ(), 2));
    }
}
