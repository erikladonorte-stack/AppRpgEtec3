using AppRpgEtec.Views.Armas;
using AppRpgEtec.Views.Personagens;

namespace AppRpgEtec
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Resgata o nome do usuário armazenado nas Preferências e exibe no rodapé do menu
            string login = Preferences.Get("UsuarioUsername", string.Empty);
            lblLogin.Text = login;

            // Registro das rotas de navegação do aplicativo
            Routing.RegisterRoute("cadPersonagemView", typeof(CadastroPersonagemView));
            Routing.RegisterRoute("cadArmaView", typeof(CadastroArmaView));
        }
    }
}