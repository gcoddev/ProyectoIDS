using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace Presentación
{
    public partial class FormPrincipal : Form
    {
        //Field
        private IconButton currentBtn;
        private Panel leftBorderBtn;
        public FormPrincipal()
        {
            InitializeComponent();
            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 60);
            panelMenu.Controls.Add(leftBorderBtn);

        }
        //Structs
        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(0,0,0);
            public static Color color2 = Color.FromArgb(249, 118, 176);
            public static Color color3 = Color.FromArgb(253, 138, 114);
            public static Color color4 = Color.FromArgb(95, 77, 221);
            public static Color color5 = Color.FromArgb(249, 88, 155);
            public static Color color6 = Color.FromArgb(24, 161, 251);
        }
        //Methods
        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                //DisableButton();
                //Button
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(253, 181, 21);
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                //Left border button
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
                //Current Child Form Icon
            }
        }
        private void DisableButton(IconButton currentBtn)
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(234, 234, 253);
                currentBtn.ForeColor = Color.Black;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.Black;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleCenter;
            }
            
        }
        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(234, 234, 253);
                currentBtn.ForeColor = Color.Black;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.Black;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleCenter;
            }

        }


        #region Funcionalidades del Formulario
        //RESIZE METODO PARA REDIMENCIONAR/CAMBIAR TAMAÑO A FORMULARIO EN TIEMPO DE EJECUCION ----------------------------------------------------------
        private int tolerance = 12;
        private const int WM_NCHITTEST = 132;
        private const int HTBOTTOMRIGHT = 17;
        private Rectangle sizeGripRectangle;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint))
                        m.Result = new IntPtr(HTBOTTOMRIGHT);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        //----------------DIBUJAR RECTANGULO / EXCLUIR ESQUINA PANEL 
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));
            sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);
            region.Exclude(sizeGripRectangle);
            this.panelContenedor.Region = region;
            this.Invalidate();
        }
        //----------------COLOR Y GRIP DE RECTANGULO INFERIOR
        protected override void OnPaint(PaintEventArgs e)
        {
            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(0, 50, 98));
            e.Graphics.FillRectangle(blueBrush, sizeGripRectangle);
            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //capturar posicion y tamaño antes de maximizar para restaurar
        int lx, ly;
        int sw, sh;
        private void btnMax_Click(object sender, EventArgs e)
        {
            lx = this.Location.X;
            ly = this.Location.Y;
            sw = this.Size.Width;
            sh= this.Size.Height;
            btnMax.Visible = false;
            btnRestaurar.Visible = true;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;  
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;
            
        }
        //METODO PARA ARRASTRAR EL FORMULARIO
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);
        private void panelContenedor_Paint(object sender, PaintEventArgs e)
        {
             
        }

        private void panelContenedor_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panelMenu_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState=FormWindowState.Minimized;
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            btnMax.Visible = true;
            btnRestaurar.Visible = false;
            this.Size = new Size(sw, sh);
            this.Location = new Point(lx, ly);
        }

        


        private void btnInventarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario<Form1>();
            ActivateButton(sender, RGBColors.color1);
            //btnInventarios.BackColor = Color.FromArgb(253, 181, 21);
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            AbrirFormulario<Form2>();
            ActivateButton(sender, RGBColors.color1);
            //btnEmpleados.BackColor = Color.FromArgb(253, 181, 21);
        }

        private void btnOficina_Click(object sender, EventArgs e)
        {
            AbrirFormulario<Form3>();
            ActivateButton(sender, RGBColors.color1);
            //btnOficina.BackColor = Color.FromArgb(253, 181, 21);
        }

        private void btnPartida_Click(object sender, EventArgs e)
        {
            AbrirFormulario<Form4>();
            ActivateButton(sender, RGBColors.color1);
            //btnPartida.BackColor = Color.FromArgb(253, 181, 21);
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FormUsuario>();
            ActivateButton(sender, RGBColors.color1);
            //btnUsuarios.BackColor = Color.FromArgb(253, 181, 21);
        }

        private void panelFormulario_Paint(object sender, PaintEventArgs e)
        {

        }


        #endregion
        //METODO PARA ABRIR FORMULARIOS DENTRO DEL PANEL
        private void AbrirFormulario<MiForm>() where MiForm : Form, new()
        {
            Form formulario;
            formulario = panelFormulario.Controls.OfType<MiForm>().FirstOrDefault();//Busca en la coleccion el formulario
                                                                                     //si el formulario/instancia no existe
            if (formulario == null)
            {
                formulario = new MiForm();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;
                panelFormulario.Controls.Add(formulario);
                panelFormulario.Tag = formulario;
                formulario.Show();
                formulario.BringToFront();
                formulario.FormClosed += new FormClosedEventHandler(CloseForms);
            }
            //si el formulario/instancia existe
            else
            {
                formulario.BringToFront();
            }
        }
        private void CloseForms(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms["Form1"] == null)
                //btnInventarios.BackColor = Color.FromArgb(234, 234, 253);
                DisableButton(btnInventarios);
            if (Application.OpenForms["Form2"] == null)
                //btnEmpleados.BackColor = Color.FromArgb(234, 234, 253);
                DisableButton(btnEmpleados);
            if (Application.OpenForms["Form3"] == null)
                //btnOficina.BackColor = Color.FromArgb(234, 234, 253);
                DisableButton(btnOficina);
            if (Application.OpenForms["Form4"] == null)
                //btnPartida.BackColor = Color.FromArgb(234, 234, 253);
                DisableButton(btnPartida);
            if (Application.OpenForms["FormUsuario"] == null)
                //btnUsuarios.BackColor = Color.FromArgb(234, 234, 253);
                DisableButton(btnUsuarios);
        }

    }
}
