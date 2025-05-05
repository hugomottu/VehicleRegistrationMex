// Arquivo JavaScript principal para o site
$(document).ready(function () {
    // Inicialização do jQuery
    console.log("Site carregado com sucesso!");

    // Função para formatar o texto de IDs de veículos
    $("#formatVehicleIds").on("click", function() {
        const rawText = $("#vehicleIdsText").val();
        if (!rawText.trim()) return;

        // Divide o texto por linhas, vírgulas ou ponto-e-vírgulas
        const lines = rawText.split(/[\r\n,;]+/);
        
        // Filtra linhas vazias e formata cada linha
        const formattedIds = lines
            .map(line => line.trim())
            .filter(line => line.length > 0)
            .join('\n');
        
        // Atualiza o textarea com o texto formatado
        $("#vehicleIdsText").val(formattedIds);
    });

    // Adiciona tooltips para elementos com data-toggle="tooltip"
    $('[data-toggle="tooltip"]').tooltip();
});
