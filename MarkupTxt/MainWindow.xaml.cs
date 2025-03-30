using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Markdig;
using MarkdownTxt.Services;

namespace MarkdownApp
{
    public partial class MainWindow : Window
    {
        private string? _caminhoArquivoAtual;
        private readonly string pastaOrigem = @"C:\Users\kcampo246\MarkdownFiles";

        public MainWindow()
        {
            InitializeComponent();
            _ = CarregarArquivosAsync();

        }

        private async Task CarregarArquivosAsync()
        {
            try
            {
                var arquivos = await ArquivoService.ObterArquivosMarkdownAsync(pastaOrigem);
                ListaArquivos.ItemsSource = arquivos;

                var primeiroArquivo = arquivos.FirstOrDefault();
                if (!string.IsNullOrEmpty(primeiroArquivo))
                {
                    ListaArquivos.SelectedItem = primeiroArquivo;
                    await CarregarArquivoAsync(Path.Combine(pastaOrigem, primeiroArquivo));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar os arquivos. {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task CarregarArquivoAsync(string caminho)
        {
            try
            {
                string markdownContent = await ArquivoService.LerArquivoAsync(caminho);
                if (string.IsNullOrEmpty(markdownContent)) return;

                TextEditor.Text = markdownContent;
                _caminhoArquivoAtual = caminho;

                await Dispatcher.InvokeAsync(() => AtualizarPreviaMarkdown(markdownContent), DispatcherPriority.Render);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar o arquivo. {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextEditor_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            AtualizarPreviaMarkdown(TextEditor.Text);
        }

        private void AtualizarPreviaMarkdown(string markdown)
        {
            string html = MarkdownService.ConverterMarkdownParaHtml(markdown);
            WebBrowserPreview.NavigateToString(html);
        }

        private async void ListaArquivos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaArquivos.SelectedItem is string nomeArquivo)
            {
                await CarregarArquivoAsync(Path.Combine(pastaOrigem, nomeArquivo));
            }
        }

        private async void CriarNovoArquivoMarkdown_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Markdown Files (*.md)|*.md",
                DefaultExt = "md",
                InitialDirectory = pastaOrigem
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    await ArquivoService.SalvarArquivoAsync(saveFileDialog.FileName, "# Novo Arquivo Markdown\n\nEste é um arquivo Markdown de exemplo.");
                    await CarregarArquivosAsync();
                    MessageBox.Show("Novo arquivo Markdown criado com sucesso!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao criar o arquivo Markdown. {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void SalvarArquivo_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_caminhoArquivoAtual))
            {
                MessageBox.Show("Nenhum arquivo selecionado para salvar.");
                return;
            }

            try
            {
                await ArquivoService.SalvarArquivoAsync(_caminhoArquivoAtual, TextEditor.Text);
                MessageBox.Show("Arquivo salvo com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar o arquivo. {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
