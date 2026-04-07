using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
	public EditarProduto()
	{
		InitializeComponent();

	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Produtos p = BindingContext as Produtos;

        if (p != null)
        {
            picker_categoria.SelectedItem = p.CategoriaTipo.ToString();
        }
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produtos produto_anexado = BindingContext as Produtos;

            Produtos p = new Produtos
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text),

                CategoriaTipo = (CategoriaTipo)Enum.Parse(
                typeof(CategoriaTipo),
                picker_categoria.SelectedItem.ToString())

            };

            await App.Db.update(p);
            await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");
            await Navigation.PopAsync();


        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}