using System;
using System.Windows;
using System.Drawing;
using System.Timers;

namespace Recordatorios
{
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon _notify = new System.Windows.Forms.NotifyIcon();
        private System.Windows.Forms.ContextMenuStrip menu;
        private Timer _timer = new Timer();
        private DateTime tiempo_del_popup = DateTime.Now;
        private bool habilitar_notificaciones = false;
        public MainWindow()
        {
            InitializeComponent();
            menu = new System.Windows.Forms.ContextMenuStrip();
            menu.Items.Add("Administrar eventos", System.Drawing.Image.FromFile("proceso.png"), new EventHandler(Admin));
            _notify.ContextMenuStrip = menu;
            _notify.Text = "Recordatorios";
            _notify.Icon = SystemIcons.Information;
            _notify.Visible = true;
            var segundos_para_esperar_y_cerrar = 5000;
            var titulo_de_la_notificacion = "Mensaje de alerta";
            var Mensaje_de_la_notificacion = "Revisar este pendiente";
            var tipo_de_icono = System.Windows.Forms.ToolTipIcon.Info;
            _notify.ShowBalloonTip(
                segundos_para_esperar_y_cerrar,
                titulo_de_la_notificacion,
                Mensaje_de_la_notificacion,
                tipo_de_icono
                );
            _timer.Interval = 1000; //cada segundo ejecutara su proceso elapsed
            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;
            _timer.Start();
        }
        private void Admin(object? sender, EventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
        private void MiEvento_Click(object sender, RoutedEventArgs e)
        {
            tiempo_del_popup = DateTime.Now.AddSeconds(int.Parse(tiempo.Text));
            habilitar_notificaciones = true;
        }
        private void _timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            var la_hora_del_sistema = DateTime.Now;
            if (habilitar_notificaciones)
            {
                if (la_hora_del_sistema > tiempo_del_popup)
                {
                    this.Dispatcher.Invoke(() => {
                        llamar_al_popup();
                    });
                }
            }
            this.Dispatcher.Invoke(() => {
                lbltime.Content = $"{la_hora_del_sistema} > {tiempo_del_popup}";
            });
        }
        private void llamar_al_popup()
        {
            var segundos_para_esperar_y_cerrar = 5000;
            var titulo_de_la_notificacion = "Mensaje de alerta";
            var Mensaje_de_la_notificacion = txtmensaje.Text;
            var tipo_de_icono = System.Windows.Forms.ToolTipIcon.Info;
            _notify.ShowBalloonTip(
                segundos_para_esperar_y_cerrar,
                titulo_de_la_notificacion,
                Mensaje_de_la_notificacion,
                tipo_de_icono
                );
            //ahora tan pronto se ejecuta la notificacion, se deshabilita para evitar q se muestre cada segundo
            habilitar_notificaciones = false;
        }
        private void Closer_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
    }
}
