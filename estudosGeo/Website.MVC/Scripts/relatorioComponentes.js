var Global = (function () {

    var instance, map, OS = [], LimiteCidade, CidadeSelected, table_relatorio_strand, table_relatorio_anotacoes, table_relatorio_edificio, table_relatorio_terreno, table_relatorio_resid;

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

    function CarregaCidades() {
        CallServer("GET", "/AjaxOrdemDeServico/GetCidades", {}, function (resposta) {
            console.log(resposta);
            var $select = $('#selectCidade');
            $.each(resposta, function (key, value) {
                $select.append('<option value=' + value.IdCidade + '>' + value.Nome + '</option>');
            });
        });
    }

    function verificaOs() {
        $("#selectOsInput").removeClass("has-error");
    }

    function OnChangeCidade() {
        $("#selectCidadeInput").removeClass("has-error");
        $('#selectOs').find('option:not(:first)').remove();
        $("#selectOs").val(0);
        var id_Cidade = $("#selectCidade").val();
        CallServer("GET", "/Relatorios/GetOsByCidadeSimples", { cidade: id_Cidade }, function (retorno) {
            console.log(retorno);
            var $select = $('#selectOs');
            $.each(retorno.Result, function (key, value) {
                $select.append('<option value=' + value.id + '>' + value.numero + '</option>');
            });
        });
    }

    function LoadStart() {
        CarregaCidades();
        table_relatorio_strand = $('#table_relatorio_strand').DataTable({ dom: 'Bfrtip', buttons: [{ extend: 'csvHtml5', title: 'Relatório de Strands' }, 'pageLength'] });
        table_relatorio_anotacoes = $('#table_relatorio_anotacoes').DataTable({ dom: 'Bfrtip', buttons: [{ extend: 'csvHtml5', title: 'Relatório de Anotacoes' }, 'pageLength'] });
        table_relatorio_edificio = $('#table_relatorio_edificio').DataTable({ dom: 'Bfrtip', buttons: [{ extend: 'csvHtml5', title: 'Relatório de Anotacoes' }, 'pageLength'] });
        table_relatorio_terreno = $('#table_relatorio_terreno').DataTable({ dom: 'Bfrtip', buttons: [{ extend: 'csvHtml5', title: 'Relatório de Anotacoes' }, 'pageLength'] });
        table_relatorio_resid = $('#table_relatorio_resid').DataTable({ dom: 'Bfrtip', buttons: [{ extend: 'csvHtml5', title: 'Relatório de Anotacoes' }, 'pageLength'] });
    }

    function StartLoad() {
        $("#div_barra_carregamento").show();
    }
    function StopLoad() {
        $("#div_barra_carregamento").hide();
    }

    function pegarDadosComponentes() {

        var IdCidade = parseInt($("#selectCidade option:selected").val());
        var idOs = parseInt($("#selectOs option:selected").val());

        var strand = $("#strands");
        var anotacoes = $("#anotacoes");
        var quadras = $("#quadras");
        var edificio = $("#edificio");
        var terreno = $("#terreno");
        var tiep = $("#tiep");
        var resid = $("#resid");
        var cni = $("#cni");
        //Verfica se é true ou false
        var strandapi = strand.is(":checked");
        var anotacoesapi = anotacoes.is(":checked");
        var quadrasapi = quadras.is(":checked");
        var edificioapi = edificio.is(":checked");
        var terrenoapi = terreno.is(":checked");
        var tiepapi = tiep.is(":checked");
        var residapi = resid.is(":checked");
        var cniapi = cni.is(":checked");

        var naoClicado = 0;
        var pode = true;

        if (strandapi) {
            $("#tableStrands").css("display", "block");
            naoClicado++;
        }
        if (anotacoesapi) {
            $("#tableAnotacoes").css("display", "block");
            naoClicado++;
        }
        if (edificioapi) {
            $("#tableEdificio").css("display", "block");
            naoClicado++;
        }
        if (terrenoapi) {
            $("#tableTerreno").css("display", "block");
            naoClicado++;
        }
        if (residapi) {
            $("#tableResid").css("display", "block");
            naoClicado++;
        }
        if (quadrasapi) {
         //   $("#tableResid").css("display", "block");
            naoClicado++;
        }
        if (tiepapi) {
            //   $("#tableResid").css("display", "block");
            naoClicado++;
        }
        if (cniapi) {
            //   $("#tableResid").css("display", "block");
            naoClicado++;
        }

        if (naoClicado == 0) {
            $("#erroCamposRel").css("display", "block");
            $("#caixaCheckBox").addClass("erroCheckboxRelatorio");
            pode = false;
        }
        if (IdCidade == 0) {
            $("#selectCidadeInput").addClass("has-error");
            pode = false;
        }

        if (idOs == 0) {
            $("#selectOsInput").addClass("has-error");
            pode = false;
        }
        if (pode) {
            $("#selectCidadeInput").removeClass("has-error");
            $("#selectOsInput").removeClass("has-error");
            $("#caixaCheckBox").removeClass("erroCheckboxRelatorio");
            $("#erroCamposRel").css("display", "none");

            if (!strandapi) {
                $("#tableStrands").css("display", "none");
            }
            if (!anotacoesapi) {
                $("#tableAnotacoes").css("display", "none");
            }
            if (!tableEdificio) {
                $("#tableAnotacoes").css("display", "none");
            }
            if (!terrenoapi) {
                $("#tableTerreno").css("display", "none");
            }
            if(!residapi){
                $("#tableResid").css("display", "none");
            }

            CallServer("GET", "/Relatorios/CriaRelatorio", {
                idCidade: IdCidade, idOs: idOs, strands: strandapi, anotacoes: anotacoesapi,
                quadras: quadrasapi, edificio: edificioapi, terreno: terrenoapi, tiep: tiepapi, resid: residapi, cni: cniapi
            }, function (retorno) {
                console.log(retorno);
                table_relatorio_strand.clear().draw();
                let rowsStrands = [];
                $.each(retorno.Result.StrandRelatorios, function (key, value) {
                    let row = [
                       value.ID,
                       value.CodigoBDGeo,
                       value.OrdemServico,
                       value.Colaborador,
                       value.Cidade,                       
                       value.X1,
                       value.Y1,
                       value.X2,
                       value.Y2
                    ];
                    rowsStrands.push(row);
                });
                table_relatorio_strand.rows.add(rowsStrands).draw();

                let rowsAnotacoes = [];
                table_relatorio_anotacoes.clear().draw();
                $.each(retorno.Result.AnotacaoRelatorios, function (key, value) {
                    let row = [
                       value.IdAnotacao,
                       value.OrdemServico,
                       value.Cidade,
                       value.Colaborador, 
                       value.Descricao,
                       value.X,
                       value.Y
                    ];
                    rowsAnotacoes.push(row);
                });
                table_relatorio_anotacoes.rows.add(rowsAnotacoes).draw();

                let rowsEidifio = [];
                table_relatorio_edificio.clear().draw();
                $.each(retorno.Result.EdificioRelatorios, function (key, value) {
                    let row = [
                       value.ID,
                       value.CodigoGeoBD,
                       value.OrdemServico,
                       value.Colaborador,
                       value.Cidade,
                       value.Numero,
                       value.NumeroAndaresEdificio,
                       value.TotalApartamentosEdificio,
                       value.NomeEdificio,
                       value.X,
                       value.Y,
                       value.Classificacao,
                       value.TipoImovel,
                       value.Complemento1,
                       value.Complemento2
                    ];
                    rowsEidifio.push(row);
                });
                table_relatorio_edificio.rows.add(rowsEidifio).draw();


                let rowsTerreno = [];
                table_relatorio_terreno.clear().draw();
                $.each(retorno.Result.TerrenoRelatorios, function (key, value) {
                    let row = [
                       value.ID,
                       value.CodigoGeoBD,
                       value.OrdemServico,
                       value.Colaborador,
                       value.Cidade,
                       value.Numero,
                       value.X,
                       value.Y,
                       value.Classificacao,
                       value.TipoImovel,
                       value.Complemento1,
                       value.Complemento2
                    ];
                    rowsTerreno.push(row);
                });
                table_relatorio_terreno.rows.add(rowsTerreno).draw();


                let rowsResid = [];
                table_relatorio_resid.clear().draw();
                $.each(retorno.Result.ReisidenciaRelatorios, function (key, value) {
                    let row = [
                       value.ID,
                       value.CodigoGeoBD,
                       value.OrdemServico,
                       value.Colaborador,
                       value.Cidade,
                       value.Numero,
                       value.X,
                       value.Y,
                       value.Classificacao,
                       value.TipoImovel,
                       value.Complemento1,
                       value.Complemento2
                    ];
                    rowsResid.push(row);
                });
                table_relatorio_resid.rows.add(rowsResid).draw();                
            });
        }
    }

    function init() {

        if (typeof (jQuery) === 'undefined') { alert("O jQuery não foi carregado. Verifique o import da libs."); return; }
        return {
            LoadStart: function () { LoadStart() },
            CarregaCidades: function () { CarregaCidades() },
            CarregarOSsByIdCidade: function (idCidade) { CarregarOSsByIdCidade(idCidade) },
            StartLoad: function() {StartLoad() },
            StopLoad: function () { StopLoad() },
            OnChangeCidade: function () { OnChangeCidade() },
            pegarDadosComponentes: function () { pegarDadosComponentes() },
            verificaOs: function(){verificaOs()}
        };
    };

    return { get: function () { if (!instance) { instance = init(); } return instance; } };

})();

Global.get().LoadStart();