using AppRpgEtec.ViewModels.Armas;

namespace AppRpgEtec.Views.Armas;

public partial class ListagemView : ContentPage
{
    private ListagemArmaViewModel viewModel;

    public ListagemView()
    {
        InitializeComponent();
        viewModel = new ListagemArmaViewModel();
        BindingContext = viewModel;
        Title = "Lista de Armas";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        collectionView.SelectedItem = null;
        _ = viewModel.ObterArmas();
    }
}