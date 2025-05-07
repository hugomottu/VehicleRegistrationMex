using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using VehicleRegistrationSystem.Constants;
using VehicleRegistrationSystem.Extensions;
using VehicleRegistrationSystem.Models;

namespace VehicleRegistrationSystem.Services
{
    public class SettingsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _settingsFilePath;

        public SettingsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            // Definir o caminho do arquivo de configurações
            _settingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        }

        public async Task<AppSettings> GetSettingsAsync()
        {
            // Primeiro, verificar se as configurações estão na sessão
            if (_httpContextAccessor.HttpContext != null)
            {
                var settings = _httpContextAccessor.HttpContext.Session.Get<AppSettings>("AppSettings");
                
                if (settings != null)
                {
                    return settings;
                }
            }

            // Se não estiver na sessão, verificar se existe o arquivo de configurações
            if (File.Exists(_settingsFilePath))
            {
                try
                {
                    var json = await File.ReadAllTextAsync(_settingsFilePath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json);
                    
                    if (settings != null && _httpContextAccessor.HttpContext != null)
                    {
                        // Salvar na sessão para acesso mais rápido
                        _httpContextAccessor.HttpContext.Session.Set("AppSettings", settings);
                    }
                    
                    return settings ?? GetDefaultSettings();
                }
                catch
                {
                    // Em caso de erro, retornar as configurações padrão
                    return GetDefaultSettings();
                }
            }

            // Se não existir arquivo, retornar as configurações padrão
            return GetDefaultSettings();
        }

        public async Task SaveSettingsAsync(AppSettings settings)
        {
            // Atualizar a data da última modificação
            settings.LastUpdated = DateTime.Now;
            
            // Salvar as configurações no arquivo
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_settingsFilePath, json);
            
            // Atualizar a sessão
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.Set("AppSettings", settings);
            }
        }

        private AppSettings GetDefaultSettings()
        {
            // Retornar as configurações padrão baseadas nas constantes
            var defaultSettings = new AppSettings
            {
                VehicleApiToken = AppConstants.VehicleApiToken,
                CoreJobApiToken = AppConstants.CoreJobApiToken,
                VehicleApiBaseUrl = AppConstants.VehicleApiBaseUrl,
                AnexosApiBaseUrl = AppConstants.AnexosApiBaseUrl,
                CoreJobApiUrl = AppConstants.CoreJobApiUrl,
                RequestApiBaseUrl = AppConstants.RequestApiBaseUrl,
                LastUpdated = DateTime.Now
            };
            
            // Salvar na sessão
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.Set("AppSettings", defaultSettings);
            }
            
            return defaultSettings;
        }
    }
}
