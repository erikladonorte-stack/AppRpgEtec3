using AppRpgEtec.Models;
using AppRpgEtec.Services.Armas;
using AppRpgEtec.Services.Personagens;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Armas
{
    [QueryProperty("ArmaSelecionadaId", "aId")]
    public class CadastroArmaViewModel : BaseViewModel
    {
        private ArmaService aService;
        private PersonagemService pService;
        private Task carregandoPersonagens;

        public CadastroArmaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new ArmaService(token);
            pService = new PersonagemService(token);

            carregandoPersonagens = ObterPersonagens();

            SalvarCommand = new Command(async () => await SalvarArma());
        }

        public ICommand SalvarCommand { get; set; }

        #region Atributos_Propriedades

        private int id;
        private string nome;
        private int dano;

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        public string Nome
        {
            get => nome;
            set { nome = value; OnPropertyChanged(nameof(Nome)); }
        }

        public int Dano
        {
            get => dano;
            set { dano = value; OnPropertyChanged(nameof(Dano)); }
        }

        private Personagem personagemSelecionado;
        public Personagem PersonagemSelecionado
        {
            get => personagemSelecionado;
            set
            {
                if (value != null)
                {
                    personagemSelecionado = value;
                    OnPropertyChanged(nameof(PersonagemSelecionado));
                }
            }
        }

        public ObservableCollection<Personagem> Personagens { get; set; }

        private string armaSelecionadaId;
        public string ArmaSelecionadaId
        {
            set
            {
                if (value != null)
                {
                    armaSelecionadaId = Uri.UnescapeDataString(value);
                    _ = CarregarArma();
                }
            }
        }

        #endregion

        #region Metodos

        public async Task ObterPersonagens()
        {
            try
            {
                Personagens = await pService.GetPersonagensAsync();
                OnPropertyChanged(nameof(Personagens));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlertAsync("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task CarregarArma()
        {
            try
            {
                await carregandoPersonagens;

                Arma a = await aService.GetArmaAsync(int.Parse(armaSelecionadaId));

                Id = a.Id;
                Nome = a.Nome;
                Dano = a.Dano;
                PersonagemSelecionado = Personagens?.FirstOrDefault(p => p.Id == a.PersonagemId);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlertAsync("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task SalvarArma()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Nome))
                    throw new Exception("Informe o nome da arma.");

                if (PersonagemSelecionado == null)
                    throw new Exception("Selecione o personagem que usará a arma.");

                Arma model = new Arma()
                {
                    Id = this.id,
                    Nome = this.nome,
                    Dano = this.dano,
                    PersonagemId = PersonagemSelecionado.Id
                };

                if (model.Id == 0)
                    await aService.PostArmaAsync(model);
                else
                    await aService.PutArmaAsync(model);

                await Application.Current.MainPage
                    .DisplayAlertAsync("Mensagem", "Dados salvos com sucesso!", "Ok");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlertAsync("Ops!", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        #endregion
    }
}