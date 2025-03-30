using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig;

namespace MarkdownTxt.Services
{
    public static class MarkdownService
    {
        public static string ConverterMarkdownParaHtml(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder().Build();
            string html = Markdown.ToHtml(markdown, pipeline);

            return EstilizarHtml(html);
        }

        private static string EstilizarHtml(string html)
        {
            return @"
            <style>
                body {
                    background-color: #333;
                    color: #E0E0E0;
                    font-family: Arial, sans-serif;
                    padding: 10px;
                }
                pre {
                    background-color: #000;
                    padding: 10px;
                    border-radius: 8px;
                    color: #00FF66;
                    overflow: auto;
                }
            </style>
            <meta charset='UTF-8'>" + html;
        }
    }
}
