using Android.Widget;
using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produtos> lista = new ObservableCollection<Produtos>();
    public string CategoriaSelecionada { get; set; }
    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();

            List<Produtos> tmp = await App.Db.getAll();

            tmp.ForEach(i => lista.Add(i));

            AplicarFiltro();

        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produtos> tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));

            AplicarFiltro();

        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            List<Produtos> todos = await App.Db.getAll();

            if (lista.Count == 0)
            {
                await DisplayAlert("Aviso", "Nenhum produto cadastrado.", "OK");
                return;
            }

            var relatorio = lista.GroupBy(p => p.CategoriaTipo).Select(g => new
            {
                Categoria = g.Key.ToString(),
                Total = g.Sum(p => p.Total)
            }).OrderByDescending(g => g.Total).ToList();

            string msg = "";

            foreach (var item in relatorio)
            {
                msg += $"{item.Categoria} {item.Total:c}\n";
            }

            double totalGeral = lista.Sum(p => p.Total);
            msg += $"\nTotal Geral {totalGeral:c}";

            await DisplayAlert("Relatório por Categoria", msg, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERRO", ex.Message, "OK");
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;

            Produtos p = selecionado.BindingContext as Produtos;

            bool confirm = await DisplayAlert("Tem certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
           await DisplayAlert("Ops", ex.Message, "OK");
        }

    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produtos p = e.SelectedItem as Produtos;

            Navigation.PushAsync(new Views.EditarProduto()
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }

    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            lista.Clear();

            List<Produtos> tmp = await App.Db.getAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private void AplicarFiltro()
    {
        if (string.IsNullOrEmpty(CategoriaSelecionada))
        {
            lst_produtos.ItemsSource = lista;
        }else if (Enum.TryParse(CategoriaSelecionada, out CategoriaTipo categoriaEnum))
        {
            lst_produtos.ItemsSource = lista
                .Where(p => p.CategoriaTipo == categoriaEnum).ToList();
        }
    }


}