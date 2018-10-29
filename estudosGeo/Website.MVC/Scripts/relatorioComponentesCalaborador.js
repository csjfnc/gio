var Global = (function () {

    var instance, map, OS = [], LimiteCidade, CidadeSelected, table_relatorio_strand, table_relatorio_anotacoes, table_relatorio_edificio, table_relatorio_terreno, table_relatorio_resid, listaDataIDS, exportData, instance2;

    function CallServer(_type, _url, _param, _method) {
        StartLoad();
        $.ajax({
            type: _type, url: _url, cache: false,
            headers: { '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
            dataType: 'json', data: _param,
            success: function (d) { _method(d); },
            error: function (jqXHR, textStatus, errorThrown) {
                alert($.parseJSON(jqXHR.responseText).Msg);
            }
        }).always(function () { StopLoad(); });
    }

    function verificaNome() {
        $("#selectNomeDiv").removeClass("has-error");
    }
    function verificaData() {
        $("#selectDataDiv").removeClass("has-error");
    }

    function verificaOs() {
        $("#selectOsDiv").removeClass("has-error");
    }
    function CarregaColaboradorPublicacoes() {
        $('#selectData').find('option:not(:first)').remove();
        CallServer("GET", "/Relatorios/ListaNomeColaboradorPublicado", {}, function (resposta) {
            console.log(resposta);
            //listaDataIDS = resposta.Result;
            var $select = $('#selectNome');
            $.each(resposta.Result, function (key, data) {
                $select.append('<option value=' + data + '>' + data + '</option>');
            });
        });
    }

    function OnChangeDataPublicacao() {
        verificaNome();
        $('#selectData').find('option:not(:first)').remove();
        $('#selectOs').find('option:not(:first)').remove();
        var user = $("#selectNome :selected").text();
        //var numeroOS = $("#selectOs :selected").text();

        var pode = true;
        if (user == "---") {
            pode = false;
        }
        if (pode) {
            CallServer("GET", "/Relatorios/ListaDatasPublicadas", { user: user }, function (resposta) {
                var selectData = $("#selectData");
                $.each(resposta.Result, function (key, data) {
                    selectData.append('<option value=' + data + '>' + data + '</option>');
                });
            });
        }
    }

    function OnChangeNumeroOs() {
        verificaData();
        $('#selectOs').find('option:not(:first)').remove();
        var namePublicao = $("#selectNome :selected").text();
        var dataPublicao = $("#selectData :selected").text();
        var selectOs = $("#selectOs");

        CallServer("GET", "/Relatorios/ListaOrdensPublicadas", { user: namePublicao, data: dataPublicao }, function (resposta) {
            var selectOs = $("#selectOs");
            $.each(resposta.Result, function (key, data) {
                selectOs.append('<option value=' + key + '>' + data + '</option>');
            });
        });
    }


    function CriaRelatorioGeral() {
        // var idOrdem = parseInt($("#selectOs").val());
        var data = $("#selectData").val();
        var os = $("#selectOs :selected").text();
        var nome = $("#selectNome :selected").val();

        var pode = true;

        if (nome == 0) {
            $("#selectNomeDiv").addClass("has-error");
            pode = false;
        } else {
            verificaNome();
        }
        if (os == "---") {
            $("#selectOsDiv").addClass("has-error");
            pode = false;
        } else {
            verificaOs();
        }
        if (data == 0) {
            $("#selectDataDiv").addClass("has-error");
            pode = false;
        } else {
            verificaData();
        }
        if (pode) {
            //CriaRelatorioStrands(data, os, nome);
            //CriaRelatorioAnotacao(data, os, nome);
            //CriaRelatorioEdificio(data, os, nome);
            //CriaRelatorioTerreno(data, os, nome);
            //CriaRelatorioResid(data, os, nome);
            //CriaRelatorioCni(data, os, nome);
            //CriaRelatorioTiep(data, os, nome);
            CriaRelatorioPoste(data, os, nome);
            CriaRelatorioPosteFotos(data, os, nome);
        }

    }

    function CriaRelatorioStrands(data, os, nome) {
        window.open('http://179.111.244.47:1010/api/mobile/RelatoriosColaborador/CriaRelatorioStrands?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioAnotacao(data, os, nome) {
        window.open('http://179.111.244.47:1010/api/mobile/RelatoriosColaborador/CriaRelatorioAnotacao?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioEdificio(data, os, nome) {
        window.open('http://179.111.244.47:1010/api/mobile/RelatoriosColaborador/CriaRelatorioEdificio?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioTerreno(data, os, nome) {
        window.open('http://179.111.244.47:1010/api/mobile/RelatoriosColaborador/CriaRelatorioTerreno?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioResid(data, os, nome) {
        window.open('http://179.111.244.47:1010/api/mobile/RelatoriosColaborador/CriaRelatorioResid?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioCni(data, os, nome) {
        window.open('http://179.111.244.47:1010/api/mobile/RelatoriosColaborador/CriaRelatorioCni?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioTiep(data, os, nome) {
        window.open('http://179.111.244.47:1010/api/mobile/RelatoriosColaborador/CriaRelatorioTiep?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioPoste(data, os, nome) {
        window.open('http://179.111.244.47:1010/api/mobile/RelatoriosColaborador/CriaRelatorioPoste?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioPosteFotos(data, os, nome) {
        window.open('http://179.111.244.47:1010/api/mobile/RelatoriosColaborador/CriaRelatorioPosteFotos?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }


    /*
    function CriaRelatorioStrands(data, os, nome) {
        window.open('http://localhost:49988/api/mobile/RelatoriosColaborador/CriaRelatorioStrands?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioAnotacao(data, os, nome) {
        window.open('http://localhost:49988/api/mobile/RelatoriosColaborador/CriaRelatorioAnotacao?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioEdificio(data, os, nome) {
        window.open('http://localhost:49988/api/mobile/RelatoriosColaborador/CriaRelatorioEdificio?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioTerreno(data, os, nome) {
        window.open('http://localhost:49988/api/mobile/RelatoriosColaborador/CriaRelatorioTerreno?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioResid(data, os, nome) {
        window.open('http://localhost:49988/api/mobile/RelatoriosColaborador/CriaRelatorioResid?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioCni(data, os, nome) {
        window.open('http://localhost:49988/api/mobile/RelatoriosColaborador/CriaRelatorioCni?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioTiep(data, os, nome) {
        window.open('http://localhost:49988/api/mobile/RelatoriosColaborador/CriaRelatorioTiep?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioPoste(data, os, nome) {
        window.open('http://localhost:49988/api/mobile/RelatoriosColaborador/CriaRelatorioPoste?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioPoste(data, os, nome) {
        window.open('http://localhost:49988/api/mobile/RelatoriosColaborador/CriaRelatorioPoste?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }
    function CriaRelatorioPosteFotos(data, os, nome) {
        window.open('http://localhost:49988/api/mobile/RelatoriosColaborador/CriaRelatorioPosteFotos?data=' + data + "&&os=" + os + "&&nome=" + nome, '_blank', '');
    }*/

    function LoadStart() {
        CarregaColaboradorPublicacoes();


        /* $("#table_relatorio_strand").tableExport({
             bootstrap: true
         });*/

    }

    function StartLoad() {
        $("#div_barra_carregamento").show();
    }
    function StopLoad() {
        $("#div_barra_carregamento").hide();
    }


    function init() {

        if (typeof (jQuery) === 'undefined') { alert("O jQuery não foi carregado. Verifique o import da libs."); return; }
        return {
            LoadStart: function () { LoadStart() },
            StartLoad: function () { StartLoad() },
            StopLoad: function () { StopLoad() },
            CarregaColaboradorPublicacoes: function () { CarregaColaboradorPublicacoes() },
            OnChangeDataPublicacao: function () { OnChangeDataPublicacao() },
            OnChangeNumeroOs: function () { OnChangeNumeroOs() },
            CriaRelatorioGeral: function () { CriaRelatorioGeral() }
        };
    };

    return { get: function () { if (!instance) { instance = init(); } return instance; } };

})();

Global.get().LoadStart();