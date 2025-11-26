using System;
using System.Globalization;

namespace CareTrack
{
    // Tipos de atividade suportados pelo aplicativo.
    enum ActivityType
    {
        Water = 1,
        SunExposure = 2,
        Stretchings = 3,
        SleepQuality = 4
    }

    // Representa um registro de atividade com tipo, data, valor numérico e texto opcional.
    struct ActivityRecord
    {
        // Tipo da atividade (enum).
        public ActivityType Type;

        // Data do registro (apenas a parte Date é utilizada).
        public DateTime Date;

        // Valor numérico associado à atividade (litros, minutos, contagem, score).
        public double Value;

        // Texto descritivo opcional (ex.: "Bom", "Médio", "Ruim").
        public string QualityText;
    }

    // Programa principal que gerencia o menu, entrada do usuário e operações sobre registros.
    class Program
    {
        // Array que guarda os registros em memória.
        static ActivityRecord[] records = new ActivityRecord[0];

        // Ponto de entrada da aplicação. Exibe a introdução e executa o loop principal do menu.
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            AppIntro();
            bool running = true;
            while (running)
            {
                DrawMobileLikeMenu();
                string choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddRecord();
                        break;
                    case "2":
                        ListRecords();
                        break;
                    case "3":
                        ShowStatistics();
                        break;
                    case "4":
                        running = false;
                        Farewell();
                        break;
                    default:
                        WriteWarning("Opção inválida — selecione 1, 2, 3 ou 4.");
                        break;
                }
            }
        }

        // Exibe cabeçalho introdutório do aplicativo.
        static void AppIntro()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║          CareTrack — Registro          ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine("Registro rápido de hábitos para promoção de saúde.\n");
        }

        // Desenha o menu principal 
        static void DrawMobileLikeMenu()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("┌────────────────────────────────────────┐");
            Console.WriteLine("│              MENU PRINCIPAL            │");
            Console.WriteLine("└────────────────────────────────────────┘");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("  [1]  Adicionar registro");
            Console.WriteLine("  [2]  Listar registros");
            Console.WriteLine("  [3]  Exibir estatísticas");
            Console.WriteLine("  [4]  Sair");
            Console.WriteLine();
            Console.Write("Escolha uma opção (1-4): ");
        }

        // Exibe mensagem de despedida.
        static void Farewell()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nObrigado por usar o CareTrack. Continue saudável! \n");
            Console.ResetColor();
        }

        // Mostra aviso em amarelo.
        static void WriteWarning(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg + "\n");
            Console.ResetColor();
        }

        // Solicita ao usuário a criação de um novo registro, valida entradas e adiciona ao array interno.
        static void AddRecord()
        {
            Console.WriteLine("=== Adicionar registro ===");

            Console.WriteLine("Selecione o tipo de atividade:");
            Console.WriteLine("  1 - Água (litros)");
            Console.WriteLine("  2 - Exposição ao Sol (minutos)");
            Console.WriteLine("  3 - Alongamentos (quantidade)");
            Console.WriteLine("  4 - Qualidade do Sono (Bom / Médio / Ruim)");
            Console.Write("Tipo (1-4): ");
            string t = Console.ReadLine()?.Trim();

            if (!int.TryParse(t, out int tChoice) || tChoice < 1 || tChoice > 4)
            {
                WriteWarning("Tipo inválido. Operação cancelada.\n");
                return;
            }

            ActivityType type = (ActivityType)tChoice;

            DateTime date = GetValidatedDate("Data (dd/MM/yyyy) ou Enter para hoje: ");

            double value = 0;
            string qualityText = null;

            try
            {
                switch (type)
                {
                    case ActivityType.Water:
                        value = GetValidatedDouble("Quantidade de água ingerida (litros, ex: 0.5): ");
                        break;

                    case ActivityType.SunExposure:
                        value = GetValidatedDouble("Tempo de exposição ao sol (minutos, ex: 15): ");
                        break;

                    case ActivityType.Stretchings:
                        int cnt = GetValidatedInt("Quantidade de alongamentos (ex: 3): ");
                        value = cnt;
                        break;

                    case ActivityType.SleepQuality:
                        Console.WriteLine("Selecione qualidade do sono:");
                        Console.WriteLine("  1 - Bom");
                        Console.WriteLine("  2 - Médio");
                        Console.WriteLine("  3 - Ruim");
                        Console.Write("Escolha (1-3): ");
                        string q = Console.ReadLine()?.Trim();
                        if (!int.TryParse(q, out int qChoice) || qChoice < 1 || qChoice > 3)
                        {
                            WriteWarning("Escolha inválida de qualidade do sono. Operação cancelada.");
                            return;
                        }
                        if (qChoice == 1) { value = 3; qualityText = "Bom"; }
                        else if (qChoice == 2) { value = 2; qualityText = "Médio"; }
                        else { value = 1; qualityText = "Ruim"; }
                        break;
                }

                ActivityRecord rec = new ActivityRecord
                {
                    Type = type,
                    Date = date.Date,
                    Value = value,
                    QualityText = qualityText
                };

                Array.Resize(ref records, records.Length + 1);
                records[records.Length - 1] = rec;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✅ Registro adicionado com sucesso!\n");
                Console.ResetColor();
            }
            catch (ArgumentException aex)
            {
                WriteWarning(aex.Message);
            }
            catch (FormatException fex)
            {
                WriteWarning(fex.Message);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro inesperado: " + ex.Message + "\n");
                Console.ResetColor();
            }
        }

        // Exibe todos os registros cadastrados em formato tabular.
        static void ListRecords()
        {
            Console.WriteLine("=== Lista de registros ===\n");
            if (records.Length == 0)
            {
                Console.WriteLine("Nenhum registro encontrado.\n");
                return;
            }

            Console.WriteLine("{0,-4} {1,-18} {2,-12} {3,10}", "ID", "Tipo", "Data", "Valor");
            Console.WriteLine(new string('-', 52));
            for (int i = 0; i < records.Length; i++)
            {
                var r = records[i];
                string typeName = ActivityTypeToLabel(r.Type);
                string valueStr = r.Type == ActivityType.SleepQuality
                    ? r.QualityText ?? ScoreToQualityText(r.Value)
                    : FormatValueByType(r.Type, r.Value);

                Console.WriteLine("{0,-4} {1,-18} {2,-12} {3,10}", i + 1, typeName, r.Date.ToString("dd/MM/yyyy"), valueStr);
            }
            Console.WriteLine();
        }

        // Calcula e apresenta estatísticas separadas para cada tipo de atividade.
        static void ShowStatistics()
        {
            Console.WriteLine("=== Estatísticas ===\n");

            if (records.Length == 0)
            {
                Console.WriteLine("Nenhum registro disponível para calcular estatísticas.\n");
                return;
            }

            double sumWater = 0; int countWater = 0;
            double sumSun = 0; int countSun = 0;
            double sumStretch = 0; int countStretch = 0;
            double sumSleepScore = 0; int countSleep = 0;
            int sleepGood = 0, sleepMedium = 0, sleepBad = 0;

            for (int i = 0; i < records.Length; i++)
            {
                var r = records[i];
                switch (r.Type)
                {
                    case ActivityType.Water:
                        sumWater += r.Value; countWater++; break;
                    case ActivityType.SunExposure:
                        sumSun += r.Value; countSun++; break;
                    case ActivityType.Stretchings:
                        sumStretch += r.Value; countStretch++; break;
                    case ActivityType.SleepQuality:
                        sumSleepScore += r.Value; countSleep++;
                        string q = r.QualityText ?? ScoreToQualityText(r.Value);
                        if (q == "Bom") sleepGood++;
                        else if (q == "Médio") sleepMedium++;
                        else sleepBad++;
                        break;
                }
            }

            Console.WriteLine("Água (litros):");
            if (countWater == 0) Console.WriteLine("  Nenhum registro.");
            else Console.WriteLine($"  Soma: {sumWater:N2} L   |   Média: {(sumWater / countWater):N2} L por registro (n={countWater})");
            Console.WriteLine();

            Console.WriteLine("Exposição ao Sol (minutos):");
            if (countSun == 0) Console.WriteLine("  Nenhum registro.");
            else Console.WriteLine($"  Soma: {sumSun:N2} min   |   Média: {(sumSun / countSun):N2} min por registro (n={countSun})");
            Console.WriteLine();

            Console.WriteLine("Alongamentos (quantidade):");
            if (countStretch == 0) Console.WriteLine("  Nenhum registro.");
            else Console.WriteLine($"  Soma: {sumStretch:N0}  |   Média: {(sumStretch / countStretch):N2} por registro (n={countStretch})");
            Console.WriteLine();

            Console.WriteLine("Qualidade do Sono (contagem / score médio):");
            if (countSleep == 0) Console.WriteLine("  Nenhum registro.");
            else
            {
                double avgScore = sumSleepScore / countSleep;
                Console.WriteLine($"  Bom: {sleepGood}  |  Médio: {sleepMedium}  |  Ruim: {sleepBad}  (total={countSleep})");
                Console.WriteLine($"  Soma (scores): {sumSleepScore:N0}  |  Score médio: {avgScore:N2} (escala: Bom=3, Médio=2, Ruim=1)");
            }
            Console.WriteLine();
        }

        // Lê uma data válida; Enter retorna a data atual.
        static DateTime GetValidatedDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    return DateTime.Today;

                if (DateTime.TryParseExact(input.Trim(),
                    new[] { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd" },
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                {
                    return dt.Date;
                }

                WriteWarning("Formato de data inválido. Use dd/MM/yyyy ou pressione Enter para hoje.");
            }
        }

        // Lê um número de ponto flutuante não-negativo; aceita vírgula ou ponto.
        static double GetValidatedDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    WriteWarning("Entrada vazia. Informe um número.");
                    continue;
                }
                string normalized = input.Trim().Replace(',', '.');
                if (double.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out double v))
                {
                    if (v < 0)
                    {
                        WriteWarning("O valor não pode ser negativo.");
                        continue;
                    }
                    return v;
                }
                WriteWarning("Formato numérico inválido. Ex: 30 ou 0.5 (aceita vírgula).");
            }
        }

        // Lê um inteiro não-negativo.
        static int GetValidatedInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    WriteWarning("Entrada vazia. Informe um número inteiro.");
                    continue;
                }
                if (int.TryParse(input.Trim(), out int v))
                {
                    if (v < 0)
                    {
                        WriteWarning("O valor não pode ser negativo.");
                        continue;
                    }
                    return v;
                }
                WriteWarning("Formato inválido. Informe um número inteiro (ex: 3).");
            }
        }

        // Converte enum para rótulo amigável.
        static string ActivityTypeToLabel(ActivityType t)
        {
            return t switch
            {
                ActivityType.Water => "Água",
                ActivityType.SunExposure => "Exposição Sol",
                ActivityType.Stretchings => "Alongamentos",
                ActivityType.SleepQuality => "Qualidade Sono",
                _ => t.ToString()
            };
        }

        // Formata valor conforme o tipo.
        static string FormatValueByType(ActivityType t, double value)
        {
            return t switch
            {
                ActivityType.Water => $"{value:N2} L",
                ActivityType.SunExposure => $"{value:N0} min",
                ActivityType.Stretchings => $"{value:N0}",
                _ => value.ToString(CultureInfo.InvariantCulture)
            };
        }

        // Converte score numérico para texto da qualidade do sono.
        static string ScoreToQualityText(double v)
        {
            if (Math.Abs(v - 3) < 0.0001) return "Bom";
            if (Math.Abs(v - 2) < 0.0001) return "Médio";
            return "Ruim";
        }
    }
}
