using WsBridge;

namespace TesteApiEntityFramework
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnListar_Click(object sender, EventArgs e)
        {
            var objRetornoNotaFiscal = await SendReceiver.GetAsync("https://localhost:7262/api/usuario/BuscarTodosUsuarios");
        }
    }
}