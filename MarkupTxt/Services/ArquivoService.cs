using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTxt.Services
{
    public static class ArquivoService
    {
        public static async Task<List<string?>> ObterArquivosMarkdownAsync(string pastaOrigem)
        {
            if (!Directory.Exists(pastaOrigem))
                return new List<string?>();

            return await Task.Run(() => Directory.GetFiles(pastaOrigem, "*.md").Select(Path.GetFileName).ToList());
        }

        public static async Task<string> LerArquivoAsync(string caminho)
        {
            if (!File.Exists(caminho)) return string.Empty;

            return await File.ReadAllTextAsync(caminho);
        }

        public static async Task SalvarArquivoAsync(string caminho, string conteudo)
        {
            await File.WriteAllTextAsync(caminho, conteudo);
        }
    }
}
