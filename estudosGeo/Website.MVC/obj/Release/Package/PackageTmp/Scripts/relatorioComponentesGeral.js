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

    function CarregaDatasPublicacoes() {
        CallServer("GET", "/Relatorios/GetListaDataOrdemServico", {}, function (resposta) {
            console.log(resposta);
            listaDataIDS = resposta.Result;
            var $select = $('#selectIDOrdemServico');
            $.each(resposta.Result, function (data, ids) {
                $select.append('<option value=' + data + '>' + data + '</option>');
            });
        });
    }

    function verificaOs() {
        $("#selectOsInput").removeClass("has-error");
    }
    function verificaData() {
        $("#selecDataOrdemServico").removeClass("has-error");
    }

    function OnChangeDataPublicacao() {
        verificaData();
        //$("#selecDataOrdemServico").removeClass("has-error");
        $('#selectOs').find('option:not(:first)').remove();
        $("#selectOs").val(0);
        var data_ordem = $("#selectIDOrdemServico").val();

        var listaIDS = listaDataIDS[data_ordem];
        var didCompleto = "";
        for (var did = 0; did < listaIDS.length; did++) {
            didCompleto += listaIDS[did] + ";";
        }

        CallServer("GET", "/Relatorios/ListaOrdemnsPorIDs", { Ids: didCompleto }, function (retorno) {
            console.log(retorno);
            var $select = $('#selectOs');
            $.each(retorno.Result, function (key, value) {
                $select.append('<option value=' + value.ID + '>' + value.Numero + '</option>');
            });
        });
    }

    function CriaRelatorioGeral() {
        var idOrdem = parseInt($("#selectOs").val());
        var data = $("#selectIDOrdemServico").val();
        var os = $("#selectOs :selected").text();       

        var pode = true;

        if (idOrdem == 0) {
            pode = false;            
            $("#selectOsInput").addClass("has-error");
        }
        if (data == 0) {
            pode = false;
            $("#selecDataOrdemServico").addClass("has-error");
        }
        if(pode){
            CriaRelatorioStrands(idOrdem, data, os);
            CriaRelatorioAnotacao(idOrdem, data, os);
            CriaRelatorioEdificio(idOrdem, data, os);
            CriaRelatorioTerreno(idOrdem, data, os);
            CriaRelatorioResid(idOrdem, data, os);
            CriaRelatorioCni(idOrdem, data, os);
            CriaRelatorioTiep(idOrdem, data, os);
            CriaRelatorioPoste(idOrdem, data, os);
        }

    }

    function CriaRelatorioStrands(idOrdem, data, os) {
        window.open('http://179.111.244.47:8080/api/mobile/Relatorioss/CriaRelatorioStrands?idOs=' + idOrdem + "&&data=" + data + "&&os=" + os, '_blank', '');
    }
    function CriaRelatorioAnotacao(idOrdem, data, os) {
        window.open('http://179.111.244.47:8080/api/mobile/Relatorioss/CriaRelatorioAnotacao?idOs=' + idOrdem + "&&data=" + data + "&&os=" + os, '_blank', '');
    }
    function CriaRelatorioEdificio(idOrdem, data, os) {
        window.open('http://179.111.244.47:8080/api/mobile/Relatorioss/CriaRelatorioEdificio?idOs=' + idOrdem + "&&data=" + data + "&&os=" + os, '_blank', '');
    }
    function CriaRelatorioTerreno(idOrdem, data, os) {
        window.open('http://179.111.244.47:8080/api/mobile/Relatorioss/CriaRelatorioTerreno?idOs=' + idOrdem + "&&data=" + data + "&&os=" + os, '_blank', '');
    }
    function CriaRelatorioResid(idOrdem, data, os) {
        window.open('http://179.111.244.47:8080/api/mobile/Relatorioss/CriaRelatorioResid?idOs=' + idOrdem + "&&data=" + data + "&&os=" + os, '_blank', '');
    }
    function CriaRelatorioCni(idOrdem, data, os) {
        window.open('http://179.111.244.47:8080/api/mobile/Relatorioss/CriaRelatorioCni?idOs=' + idOrdem + "&&data=" + data + "&&os=" + os, '_blank', '');
    }
    function CriaRelatorioTiep(idOrdem, data, os) {
        window.open('http://179.111.244.47:8080/api/mobile/Relatorioss/CriaRelatorioTiep?idOs=' + idOrdem + "&&data=" + data + "&&os=" + os, '_blank', '');
    }
    function CriaRelatorioPoste(idOrdem, data, os) {
        window.open('http://179.111.244.47:8080/api/mobile/Relatorioss/CriaRelatorioPoste?idOs=' + idOrdem + "&&data=" + data + "&&os=" + os, '_blank', '');
    }

    function pegarDadosComponentes() {

        $('#anotacoes_relatorio tbody').empty();
        $('#strand_relatorio tbody').empty();
        $('#edificio_relatorio tbody').empty();
        $('#terreno_relatorio tbody').empty();
        $('#resid_relatorio tbody').empty();
        $('#cni_relatorio tbody').empty();
        $('#tiep_relatorio tbody').empty();
        $('#poste_relatorio tbody').empty();        

        var idOrdem = parseInt($("#selectOs").val());
        var dataPublicacao = $("#selectIDOrdemServico").val();
        var numeroOS = $("#selectOs :selected").text();
        var erroCamposRel = $("#erroCamposRel");

        var pode = true;

        if (dataPublicacao == 0) {
            $("#selecDataOrdemServico").addClass("has-error");
            pode = false;
        } if (idOrdem == 0) {
            $("#selectOsInput").addClass("has-error");
            pode = false;
        }
        if (pode) {
            erroCamposRel.css("display", "none");
            CallServer("GET", "/Relatorios/CriaRelatorioGeral", {
                idOs: idOrdem
            }, function (retorno) {
                console.log(retorno);
                let rowsStrands = [];

                $.each(retorno.Result.StrandRelatorios, function (key, item) {
                    var rows = "<tr>"
                   + "<td class='prtoducttd'>" + item.ID + "</td>"
                   + "<td class='prtoducttd'>" + item.X1 + "</td>"
                   + "<td class='prtoducttd'>" + item.Y1 + "</td>"
                   + "<td class='prtoducttd'>" + item.X2 + "</td>"
                   + "<td class='prtoducttd'>" + item.Y2 + "</td>"
                   + "<td class='prtoducttd'>" + item.Ativo + "</td>"
                   + "<td class='prtoducttd'>" + item.OrdemServico + "</td>"

                   + "</tr>";
                    $('#strand_relatorio tbody').append(rows);

                });

                $.each(retorno.Result.AnotacaoRelatorios, function (key, item) {
                    var rows = "<tr>"
                   + "<td class='prtoducttd'>" + item.IdAnotacao + "</td>"
                   + "<td class='prtoducttd'>" + item.Descricao + "</td>"
                   + "<td class='prtoducttd'>" + item.X + "</td>"
                   + "<td class='prtoducttd'>" + item.Y + "</td>"
                   + "<td class='prtoducttd'>" + item.Angulo + "</td>"
                   + "<td class='prtoducttd'>" + item.Ativo + "</td>"
                   + "<td class='prtoducttd'>" + item.OrdemServico + "</td>"

                   + "</tr>";
                    $('#anotacoes_relatorio tbody').append(rows);
                });


                $.each(retorno.Result.EdificioRelatorios, function (key, item) {
                    var rows = "<tr>"
                   + "<td class='prtoducttd'>" + item.ID + "</td>"
                   + "<td class='prtoducttd'>" + item.Classificacao + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.Numero + "</td>"
                   + "<td class='prtoducttd'>" + item.Complemento1 + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.Complemento2 + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.X + "</td>"
                   + "<td class='prtoducttd'>" + item.Y + "</td>"
                   + "<td class='prtoducttd'>" + item.CodPoste + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.NumeroAndaresEdificio + "</td>"
                   + "<td class='prtoducttd'>" + item.TotalApartamentosEdificio + "</td>"
                   + "<td class='prtoducttd'>" + item.NomeEdificio + "</td>"
                   + "<td class='prtoducttd'>" + item.Ativo + "</td>"
                   + "<td class='prtoducttd'>" + item.AnoLevantamentoEdificio + "</td>"
                   + "<td class='prtoducttd'>" + item.Angulo + "</td>"
                   + "<td class='prtoducttd'>" + item.OrdemServico + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"

                   + "</tr>";
                    $('#edificio_relatorio tbody').append(rows);

                });

                $.each(retorno.Result.TerrenoRelatorios, function (key, item) {
                    var rows = "<tr>"
                   + "<td class='prtoducttd'>" + item.ID + "</td>"
                   + "<td class='prtoducttd'>" + item.Classificacao + "</td>"
                   + "<td class='prtoducttd'>" + item.Numero + "</td>"
                   + "<td class='prtoducttd'>" + item.TipoImovel + "</td>"
                   + "<td class='prtoducttd'>" + item.X + "</td>"
                   + "<td class='prtoducttd'>" + item.Y + "</td>"
                   + "<td class='prtoducttd'>" + item.CodPoste + "</td>"
                   + "<td class='prtoducttd'>" + item.Ativo + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.Angulo + "</td>"
                   + "<td class='prtoducttd'>" + item.Divisao + "</td>"
                   + "<td class='prtoducttd'>" + item.OrdemServico + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.PosteBox + "</td>"


                   + "</tr>";
                    $('#terreno_relatorio tbody').append(rows);
                });


                $.each(retorno.Result.ReisidenciaRelatorios, function (key, item) {
                    var rows = "<tr>"
                   + "<td class='prtoducttd'>" + item.ID + "</td>"
                   + "<td class='prtoducttd'>" + item.Classificacao + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.Numero + "</td>"
                   + "<td class='prtoducttd'>" + item.Complemento1 + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.Complemento2 + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.X + "</td>"
                   + "<td class='prtoducttd'>" + item.Y + "</td>"
                   + "<td class='prtoducttd'>" + item.CodPoste + "</td>"
                   + "<td class='prtoducttd'>" + item.Divisao + "</td>"
                   + "<td class='prtoducttd'>" + item.Qtdedem + "</td>"
                   + "<td class='prtoducttd'>" + item.Ativo + "</td>"
                   + "<td class='prtoducttd'>" + item.OrdemServico + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.PosteBox + "</td>"

                   + "</tr>";
                    $('#resid_relatorio tbody').append(rows);

                });

                $.each(retorno.Result.ComercioRelatorios, function (key, item) {
                    var rows = "<tr>"
                   + "<td class='prtoducttd'>" + item.ID + "</td>"
                   + "<td class='prtoducttd'>" + item.Classificacao + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.Numero + "</td>"
                   + "<td class='prtoducttd'>" + item.Complemento1 + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.Complemento2 + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.X + "</td>"
                   + "<td class='prtoducttd'>" + item.Y + "</td>"
                   + "<td class='prtoducttd'>" + item.CodPoste + "</td>"
                   + "<td class='prtoducttd'>" + item.Angulo + "</td>"
                   + "<td class='prtoducttd'>" + item.Divisao + "</td>"
                   + "<td class='prtoducttd'>" + item.Qtdedem + "</td>"
                   + "<td class='prtoducttd'>" + item.Ativo + "</td>"
                   + "<td class='prtoducttd'>" + item.OrdemServico + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.PosteBox + "</td>"

                   + "</tr>";
                    $('#cni_relatorio tbody').append(rows);

                });

                $.each(retorno.Result.TiepRelatorios, function (key, item) {
                    var rows = "<tr>"
                   + "<td class='prtoducttd'>" + item.ID + "</td>"
                   + "<td class='prtoducttd'>" + item.X1 + "</td>"
                   + "<td class='prtoducttd'>" + item.Y1 + "</td>"
                   + "<td class='prtoducttd'>" + item.X2 + "</td>"
                   + "<td class='prtoducttd'>" + item.Y2 + "</td>"
                   + "<td class='prtoducttd'>" + item.Ativo + "</td>"
                   + "<td class='prtoducttd'>" + item.DemRel + "</td>"
                   + "<td class='prtoducttd'>" + item.Layer + "</td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'></td>"
                   + "<td class='prtoducttd'>" + item.OrdemServico + "</td>"
                   + "<td class='prtoducttd'></td>"

                   + "</tr>";
                    $('#tiep_relatorio tbody').append(rows);

                });

                $.each(retorno.Result.PostesRelatorios, function (key, item) {
                    var rows = "<tr>"
                   + "<td class='prtoducttd'>" + item.Codigo + "</td>"
                   + "<td class='prtoducttd'>" + item.Logradouro + "</td>"
                   + "<td class='prtoducttd'>" + item.Equipamento1 + "</td>"
                   + "<td class='prtoducttd'>" + item.Equipamento2 + "</td>"
                   + "<td class='prtoducttd'>" + item.Equipamento3 + "</td>"
                   + "<td class='prtoducttd'>" + item.Aterramento + "</td>"
                   + "<td class='prtoducttd'>" + item.Status + "</td>"
                   + "<td class='prtoducttd'>" + item.Nomedobloco + "</td>"
                   + "<td class='prtoducttd'>" + item.X + "</td>"
                   + "<td class='prtoducttd'>" + item.Y + "</td>"
                   + "<td class='prtoducttd'>" + item.Id_temp + "</td>"
                   + "<td class='prtoducttd'>" + item.Ativo + "</td>"
                   + "<td class='prtoducttd'>" + item.Primario + "</td>"
                   + "<td class='prtoducttd'>" + item.Servico + "</td>"
                   + "<td class='prtoducttd'>" + item.Lat + "</td>"
                   + "<td class='prtoducttd'>" + item.Longitude + "</td>"
                   + "<td class='prtoducttd'>" + item.Node + "</td>"
                   + "<td class='prtoducttd'>" + item.Proprietario + "</td>"
                   + "<td class='prtoducttd'>" + item.Tecnico + "</td>"
                   + "<td class='prtoducttd'>" + item.Data + "</td>"
                   + "<td class='prtoducttd'>" + item.Municipio + "</td>"
                   + "<td class='prtoducttd'>" + item.X_original + "</td>"
                   + "<td class='prtoducttd'>" + item.Y_original + "</td>"
                   + "<td class='prtoducttd'>" + item.Status_edicao + "</td>"
                   + "<td class='prtoducttd'>" + item.Cod_geodatabase + "</td>"
                   + "<td class='prtoducttd'>" + item.Id_poste_arcitech + "</td>"
                   + "<td class='prtoducttd'>" + item.Quantidade_poste + "</td>"
                   + "<td class='prtoducttd'>" + item.Idpostecia + "</td>"
                   + "<td class='prtoducttd'>" + item.Caracteristica_cia + "</td>"
                   + "<td class='prtoducttd'>" + item.Aterropararaio_cia + "</td>"
                   + "<td class='prtoducttd'>" + item.Encontrado + "</td>"
                   + "<td class='prtoducttd'>" + item.Tipo_poste + "</td>"
                   + "<td class='prtoducttd'>" + item.Material_poste + "</td>"
                   + "<td class='prtoducttd'>" + item.Altura_poste + "</td>"
                   + "<td class='prtoducttd'>" + item.Esforco_poste + "</td>"
                   + "<td class='prtoducttd'>" + item.Tipo_base + "</td>"
                   + "<td class='prtoducttd'>" + item.Para_raio + "</td>"
                   + "<td class='prtoducttd'>" + item.Estai + "</td>"
                   + "<td class='prtoducttd'>" + item.Observacao + "</td>"
                   + "<td class='prtoducttd'>" + item.Qtde_ramalligacao + "</td>"
                   + "<td class='prtoducttd'>" + item.Qtde_ramalservico + "</td>"
                   + "<td class='prtoducttd'>" + item.Qtde_estai + "</td>"
                   + "<td class='prtoducttd'>" + item.Avaria + "</td>"
                   + "<td class='prtoducttd'>" + item.Ocupantes + "</td>"
                   + "<td class='prtoducttd'>" + item.Qtde_ocp + "</td>"
                   + "<td class='prtoducttd'>" + item.Qtde_drop + "</td>"
                   + "<td class='prtoducttd'>" + item.Estai2 + "</td>"
                   + "<td class='prtoducttd'>" + item.Qtde_estai2 + "</td>"
                   + "<td class='prtoducttd'>" + item.Lampsemaforo + "</td>"
                   + "<td class='prtoducttd'>" + item.Tipo_zona + "</td>"  

                   + "</tr>";
                    $('#poste_relatorio tbody').append(rows);

                });

                var total = 0;


                //Relatorios de Strands
                if (retorno.Result.StrandRelatorios != 0) {
                    var ExportButtons = $('#strand_relatorio');

                    instance2 = new TableExport(ExportButtons, {
                        formatos: ['csv'],
                        exportButtons: false,
                    });
                    exportData = instance2.getExportData()['strand_relatorio']['csv'];
                    exportData.filename = "strand-" + numeroOS + "-" + dataPublicacao;
                    instance2.export2file(exportData.data, exportData.mimeType, exportData.filename, exportData.fileExtension);

                } else {
                    total++;
                }

                //Relatórios de Anotação
                if (retorno.Result.AnotacaoRelatorios != 0) {
                    var ExportButtons = $('#anotacoes_relatorio');
                    instance2 = new TableExport(ExportButtons, {
                        formatos: ['csv'],
                        exportButtons: false
                    });
                    exportData = instance2.getExportData()['anotacoes_relatorio']['csv'];
                    exportData.filename = "anotacoes-" + numeroOS + "-" + dataPublicacao;
                    instance2.export2file(exportData.data, exportData.mimeType, exportData.filename, exportData.fileExtension);
                } else {
                    total++;
                }
                //Relatórios de Edificio
                if (retorno.Result.EdificioRelatorios != 0) {
                    var ExportButtons = $('#edificio_relatorio');
                    instance2 = new TableExport(ExportButtons, {
                        formatos: ['csv'],
                        exportButtons: false
                    });
                    exportData = instance2.getExportData()['edificio_relatorio']['csv'];
                    exportData.filename = "edificio-" + numeroOS + "-" + dataPublicacao;
                    instance2.export2file(exportData.data, exportData.mimeType, exportData.filename, exportData.fileExtension);
                } else {
                    total++;
                }

                //Relatórios de Terreno
                if (retorno.Result.TerrenoRelatorios != 0) {
                    var ExportButtons = $('#terreno_relatorio');
                    instance2 = new TableExport(ExportButtons, {
                        formatos: ['csv'],
                        exportButtons: false
                    });
                    exportData = instance2.getExportData()['terreno_relatorio']['csv'];
                    exportData.filename = "terreno-" + numeroOS + "-" + dataPublicacao;
                    instance2.export2file(exportData.data, exportData.mimeType, exportData.filename, exportData.fileExtension);
                } else {
                    total++;
                }
                //Relatórios de Residencia
                if (retorno.Result.ReisidenciaRelatorios != 0) {
                    var ExportButtons = $('#resid_relatorio');
                    instance2 = new TableExport(ExportButtons, {
                        formatos: ['csv'],
                        exportButtons: false
                    });
                    exportData = instance2.getExportData()['resid_relatorio']['csv'];
                    exportData.filename = "resid-" + numeroOS + "-" + dataPublicacao;
                    instance2.export2file(exportData.data, exportData.mimeType, exportData.filename, exportData.fileExtension);
                } else {
                    total++;
                }
                //Relatórios de Comercio
                if (retorno.Result.ComercioRelatorios != 0) {
                    var ExportButtons = $('#cni_relatorio');
                    instance2 = new TableExport(ExportButtons, {
                        formatos: ['csv'],
                        exportButtons: false
                    });
                    exportData = instance2.getExportData()['cni_relatorio']['csv'];
                    exportData.filename = "cni-" + numeroOS + "-" + dataPublicacao;
                    instance2.export2file(exportData.data, exportData.mimeType, exportData.filename, exportData.fileExtension);
                } else {
                    total++;
                }
                //Relatórios de Tiep
                if (retorno.Result.TiepRelatorios != 0) {
                    var ExportButtons = $('#tiep_relatorio');
                    instance2 = new TableExport(ExportButtons, {
                        formatos: ['csv'],
                        exportButtons: false
                    });
                    exportData = instance2.getExportData()['tiep_relatorio']['csv'];
                    exportData.filename = "tiep-" + numeroOS + "-" + dataPublicacao;
                    instance2.export2file(exportData.data, exportData.mimeType, exportData.filename, exportData.fileExtension);
                } else {
                    total++;
                }

                //Relatórios de Postes
                if (retorno.Result.PostesRelatorios != 0) {
                    var ExportButtons = $('#poste_relatorio');
                    instance2 = new TableExport(ExportButtons, {
                        formatos: ['csv'],
                        exportButtons: false
                    });
                    exportData = instance2.getExportData()['poste_relatorio']['csv'];
                    exportData.filename = "poste-" + numeroOS + "-" + dataPublicacao;
                    instance2.export2file(exportData.data, exportData.mimeType, exportData.filename, exportData.fileExtension);
                } else {
                    total++;
                }

                if(total == 8){                    
                    erroCamposRel.css("display", "block");
                } else {
                    erroCamposRel.css("display", "none");
                }
            });
        }
       }
        
    function LoadStart() {
        CarregaDatasPublicacoes();
       

       /* $("#table_relatorio_strand").tableExport({
            bootstrap: true
        });*/
        
    }

    function expotar() {
       
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
            CarregaDatasPublicacoes: function () { CarregaDatasPublicacoes() },
            CarregarOSsByIdCidade: function (idCidade) { CarregarOSsByIdCidade(idCidade) },
            StartLoad: function () { StartLoad() },
            StopLoad: function () { StopLoad() },
            OnChangeDataPublicacao: function () { OnChangeDataPublicacao() },
            verificaOs: function () { verificaOs() },
            verificaData: function () { verificaData() },
            pegarDadosComponentes: function () { pegarDadosComponentes() },
            expotar: function () { expotar()},
            CriaRelatorioGeral: function () { CriaRelatorioGeral() }
        };
    };

    return { get: function () { if (!instance) { instance = init(); } return instance; } };

})();

Global.get().LoadStart();