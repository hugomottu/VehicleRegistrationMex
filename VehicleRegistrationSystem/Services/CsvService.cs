using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using VehicleRegistrationSystem.Models;

namespace VehicleRegistrationSystem.Services
{
    public class CsvService
    {
        public List<NewVehicle> ProcessCsvFile(Stream fileStream)
        {
            try
            {
                Console.WriteLine("Iniciando processamento do arquivo CSV");
                
                // Cria uma cópia do stream para debug
                var memoryStream = new MemoryStream();
                fileStream.CopyTo(memoryStream);
                memoryStream.Position = 0;
                
                // Lê o conteúdo do arquivo para debug
                using (var reader = new StreamReader(memoryStream, Encoding.UTF8, true))
                {
                    var fileContent = reader.ReadToEnd();
                    Console.WriteLine($"Conteúdo do arquivo: {fileContent}");
                    
                    // Verifica se o arquivo está vazio
                    if (string.IsNullOrWhiteSpace(fileContent))
                    {
                        Console.WriteLine("Arquivo CSV está vazio");
                        return new List<NewVehicle>();
                    }
                }
                
                // Reseta a posição do stream original
                fileStream.Position = 0;
                
                // Configura o CsvReader com opções mais flexíveis
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    HasHeaderRecord = true,
                    MissingFieldFound = null,
                    HeaderValidated = null,
                    BadDataFound = null,
                    IgnoreBlankLines = true,
                    TrimOptions = TrimOptions.Trim
                };
                
                using (var reader = new StreamReader(fileStream, Encoding.UTF8, true))
                using (var csv = new CsvReader(reader, config))
                {
                    try
                    {
                        // Mapeia as colunas do CSV para as propriedades do modelo
                        csv.Context.RegisterClassMap<NewVehicleMap>();
                        
                        // Lê todos os registros do CSV
                        var records = csv.GetRecords<NewVehicle>().ToList();
                        Console.WriteLine($"Registros lidos com sucesso: {records.Count}");
                        
                        foreach (var record in records)
                        {
                            Console.WriteLine($"Registro: Conteiner={record.ContainerNumber}, Chassi={record.Chassis}, Motor={record.Engine}");
                        }
                        
                        return records;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao ler registros do CSV: {ex.Message}");
                        Console.WriteLine($"Stack trace: {ex.StackTrace}");
                        return new List<NewVehicle>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção ao processar arquivo CSV: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return new List<NewVehicle>();
            }
        }
    }

    // Classe para mapear as colunas do CSV para as propriedades do modelo
    public class NewVehicleMap : ClassMap<NewVehicle>
    {
        public NewVehicleMap()
        {
            Map(m => m.ContainerNumber).Name("numero do conteiner");
            Map(m => m.Chassis).Name("chassi");
            Map(m => m.Engine).Name("motor");
        }
    }
}
