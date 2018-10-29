$.GetParamUrl = function (name) { var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href); if (results == null) { return null; } else { return results[1] || 0; } }

function PosteMarker(p, map) {
    this.IdPoste = p.IdPoste;
    this.Latitude = p.Latitude;
    this.Longitude = p.Longitude;
    this.NomeImagem = p.Img;
    this.CodGeo = p.CodGeo;
    this.NumeroPosteNaOS = p.NumeroPosteNaOS;
    var NumeroPoste = p.IdPoste;
    this.marker = new google.maps.Marker({ 
    	position: new google.maps.LatLng(this.Latitude, this.Longitude), 
    	map: map, 
    	title: 'Código do Poste ' + this.CodGeo + ' / Número GPS ' + this.NumeroPosteNaOS, 
    	icon: 'Images/' + this.NomeImagem + '.png' 
    });
    this.marker.setMap(map);
    var CodigoGeo = p.CodGeo;
    google.maps.event.addListener(this.marker, 'click', function (event) { $('#lb_IdPoste').text(CodigoGeo); $('#hiden_idPoste').val(NumeroPoste); $('#hiden_codGeo').val(CodigoGeo); GlobalMaps.get().MontarModalOpcoes(); });
    this.Remove = function () { this.marker.setMap(null) }
}

function PontoEntregaMarker(p, map) {
    this.IdPontoEntrega = p.IdPontoEntrega;
    this.Latitude = p.Latitude;
    this.Longitude = p.Longitude;
    this.NomeImagem = p.Img;
    this.CodGeo = p.CodGeo;
    this.Numero = p.Numero;
    this.NumeroPosteNaOS = p.NumeroPosteNaOS;
    var IdDemanda = p.IdPontoEntrega;
    this.marker = new google.maps.Marker({
        position: new google.maps.LatLng(this.Latitude, this.Longitude),
        map: map,
        title: 'Numero: ' + this.Numero,
        icon: 'Images/' + this.NomeImagem + '.png'
    });
    this.marker.setMap(map);
    var CodigoGeo = p.CodGeo;
    google.maps.event.addListener(this.marker, 'click', function (event)
    {
        $('#lb_IdPoste').text(CodigoGeo); $('#hiden_idDemanda').val(IdDemanda); $('#hiden_codGeo').val(CodigoGeo); GlobalMaps.get().MontarModalPontoOpcoes();
    });

    this.Remove = function () { this.marker.setMap(null) }
}

function AnotacaoMarker(p, map) {
    this.IdAnotacao = p.ID;
    this.X = p.X;
    this.Y = p.Y;
    this.Descricao = p.Descricao,
    this.CodGeo = p.CodGeo;   
    this.marker = new google.maps.Marker({
        position: new google.maps.LatLng(this.X, this.Y),
        map: map,
        title: 'Numero: ' + this.Descricao,
        icon: 'Images/notas.png'
    });
    this.marker.setMap(map);
    var CodigoGeo = p.CodGeo;
    google.maps.event.addListener(this.marker, 'click', function (event) {
        $('#lb_IdPoste').text(CodigoGeo); $('#hiden_idAnotocao').val(IdDemanda); $('#hiden_codGeo').val(CodigoGeo); GlobalMaps.get().MontarModalPontoOpcoes();
    });

    this.Remove = function () { this.marker.setMap(null) }
}
/*
function VaosDemandasPolyline(p, map) {
    this.IdVaosDemandas = p.IdVaosDemandas;

    var x1 = p.X1;
    var y1 = p.Y1;
    var x2 = p.X2;
    var y2 = p.Y2;

    var vaos = [
         { lat: x1, lng: y1 },
         { lat: x2, lng: y2 }
    ];

    this.polyline = new google.maps.Polyline({
        path: vaos,
        geodesic: true,
        strokeColor: '#FF0000',
        strokeOpacity: 1.0,
        strokeWeight: 2
    });
    this.polyline.setMap(map);
    var CodigoGeo = p.CodGeo;
    /*google.maps.event.addListener(this.marker, 'click', function (event) { $('#lb_IdPoste').text(CodigoGeo); $('#hiden_idPoste').val(NumeroPoste); $('#hiden_codGeo').val(CodigoGeo); GlobalMaps.get().MontarModalOpcoes(); });
    this.Remove = function () { this.marker.setMap(null) }*/
//}*/

function Limetes(coordenadas, map) {
    this.Linhas = [];

    if (coordenadas.length > 0) {
        for (var i = 0; i < coordenadas.length; i++) {
            var position = [{ lat: (coordenadas[i].LatitudeA), lng: (coordenadas[i].LongitudeA) }, { lat: (coordenadas[i].LatitudeB), lng: (coordenadas[i].LongitudeB) }];

            var Line = new google.maps.Polyline({
                path: position,
                geodesic: true,
                strokeColor: '#000099',
                strokeOpacity: 1.0,
                strokeWeight: 3
            });

            Line.setMap(map);
            this.Linhas.push(Line);
        }
    }

    this.Remove = function () {
        if (this.Linhas != null) {
            for (var i = 0; i < this.Linhas.length; i++) {
                this.Linhas[i].setMap(null);
            }
        }
    };
}

function LimetesQuadrasModel(quadras, map) {
    this.LinhaQuadras = [];

    if (quadras.length > 0) {
        for (var i = 0; i < quadras.length; i++) {

            let position = [
                {
                    lat: (quadras[i].LatitudeA),
                    lng: (quadras[i].LongitudeA)
                },
                {
                    lat: (quadras[i].LatitudeB),
                    lng: (quadras[i].LongitudeB)
                }
            ];

            let Line = new google.maps.Polyline({
                path: position,
                geodesic: true,
                strokeColor: '#777777',
                strokeOpacity: 1,
                strokeWeight: 3
            });

            Line.setMap(map);
            this.LinhaQuadras.push(Line);
        }
    }

    this.Remove = function () {
        if (this.LinhaQuadras != null) {
            for (var i = 0; i < this.LinhaQuadras.length; i++) {
                this.LinhaQuadras[i].setMap(null);
            }
        }
    };
}

function PontoEntregaMapa(coordenadas,map){

	var positionLine = [{ lat: (coordenadas.poste_latitude), lng: (coordenadas.poste_longitude) }, { lat: (coordenadas.ponto_entrega_latitude), lng: (coordenadas.ponto_entrega_longitude) }];

	this.line = new google.maps.Polyline({
		path: positionLine,
		geodesic: true,
		strokeColor: '#000099',
		strokeOpacity: 1.0,
		strokeWeight: 3,
		map: map
	});

  	this.marker = new google.maps.Marker({
	    position: {lat: coordenadas.ponto_entrega_latitude, lng: coordenadas.ponto_entrega_longitude},
	    map: map,
	    title: 'Ponto Entrega ' + coordenadas.IdPontoEntrega,
	    icon: 'Images/' + coordenadas.Image,
  	});

    this.Remove = function () {
		this.marker.setMap(null);
		this.line.setMap(null);
	};
}

function VaoMapa(coordenadas,map){

	var positionLine = [{ lat: (coordenadas.LatitudeA), lng: (coordenadas.LongitudeA) }, { lat: (coordenadas.LatitudeB), lng: (coordenadas.LongitudeB) }];

	this.line = new google.maps.Polyline({
		path: positionLine,
		geodesic: true,
		strokeColor: '#ff0000',
		strokeOpacity: 1.0,
		strokeWeight: 3,
		map: map
	});

    this.Remove = function () {
		this.line.setMap(null);
	};
}

function ArvoreMapa(dados, map) {

    this.marker = new google.maps.Marker({
        position: { lat: dados.Latitude, lng: dados.Longitude },
        map: map,
        title: 'Árvore ' + dados.IdArvore,
        icon: 'Images/tree.png'
    });

    google.maps.event.addListener(this.marker, 'click', function (event) {
        GlobalMaps.get().OpenModalArvore(dados.IdArvore);
    });
    this.Remove = function () { this.marker.setMap(null) }

    this.Remove = function () {
        this.marker.setMap(null);
    };

}

function VaoDemandas(coordenadas, map) {

    var positionLine = [{ lat: (coordenadas.X1), lng: (coordenadas.Y1) }, { lat: (coordenadas.X2), lng: (coordenadas.Y2) }];
    this.ID = coordenadas.ID;
    this.line = new google.maps.Polyline({
        path: positionLine,
        geodesic: true,
        strokeColor: '#ff0000',
        strokeOpacity: 1.0,
        strokeWeight: 2,
        map: map
    });

    this.Remove = function () {
        this.line.setMap(null);
    };
}

function Strands(coordenadas, map) {

    var positionLine = [{ lat: (coordenadas.X1), lng: (coordenadas.Y1) }, { lat: (coordenadas.X2), lng: (coordenadas.Y2) }];
    this.ID = coordenadas.ID;
    this.line = new google.maps.Polyline({
        path: positionLine,
        geodesic: true,
        strokeColor: '#ff9042',
        strokeOpacity: 1.0,
        strokeWeight: 4,
        map: map
    });

    this.Remove = function () {
        this.line.setMap(null);
    };
}

var GlobalMaps = (function () {
    var instance;
    var map;
    var RedePostes = [];
    var Id_Cidade;
    var MakerAux;
    var TipoFiltro;
    var LimiteCidade;
    var RedePontoEntrega = [];
    var RedeVaosDemanda = [];
    var RedeAnotacao = [];
    var RedeStrands = [];
    var RedeVaos = [];
    var Arvores = [];
    var MakerDemandaAux;
    var LimetesQuadras = null;;


    /// Variaveis Para Foto
    var cont_foto = 0;
    var identificadores_foto = [];
    var fotos_objetos = [];

    /// Variaveis para Medidodes
    var medidores_objetos = [];
    var contador_medidor = 0;
	
	/// Variaveis para a foto do Ponto de Entrega
	var cont_foto_ponto = 0;
    var identificadores_foto_ponto = [];
    var fotos_objetos_ponto = [];

    ///Variaveis Arvore
    Lat_arvore = 0;
    Lon_arvore = 0;
	

    //Add postes no map e no array de postes do java script
    function CreatePostes(Result) {
        //if (Result.Postes.length == 0) { bootbox.alert("Não existe Postes para esta consulta."); return; }
        var quantNA = 0;
        var quantSA = 0;
        for (var i = 0; i < Result.Postes.length; i++) {
			//caso um filtro esteja sendo aplicado os postes sao submetidos a esses metodos.
			if (TipoFiltro != undefined && TipoFiltro != ""){
				if(TipoFiltro == "Potencia"){ Result.Postes[i].Img = AtribuiImgFiltroPotencia(Result.Postes[i].Img); }
				else if(TipoFiltro == "Lampada"){ Result.Postes[i].Img = AtribuiImgFiltroLampada(Result.Postes[i].Img); }
				else if (TipoFiltro == "Status") { Result.Postes[i].Img = Result.Postes[i].Img; }
				else if (TipoFiltro == "NaSa") {
				    Result.Postes[i].Img = Result.Postes[i].Img;
				    if (Result.Postes[i].Img == "17") { quantNA++; }
                    else if (Result.Postes[i].Img == "19") {quantSA++; }
				}
			}
            RedePostes.push(new PosteMarker(Result.Postes[i], map));
        }
        if (TipoFiltro == "NaSa") {
            MontaLegendaNaSa(quantNA, quantSA);
        }

        map.setZoom(14);
        if (Result.Centro.Lat && Result.Centro.Lon) { map.setCenter(new google.maps.LatLng(Result.Centro.Lat, Result.Centro.Lon)); }
        HideAllModal();
    }

    //Add postes no map e no array de postes do java script
    function CreatePontoEntrega(Result) {
       // if (Result.PontoEntregas.length == 0) { bootbox.alert("Não existe Postes para esta consulta."); return; }
        var quantNA = 0;
        var quantSA = 0;
        for (var i = 0; i < Result.PontoEntregas.length; i++) {
            //caso um filtro esteja sendo aplicado os postes sao submetidos a esses metodos.
            if (TipoFiltro != undefined && TipoFiltro != "") {
                if (TipoFiltro == "Potencia") { Result.PontoEntregas[i].Img = AtribuiImgFiltroPotencia(Result.PontoEntregas[i].Img); }
                else if (TipoFiltro == "Lampada") { Result.PontoEntregas[i].Img = AtribuiImgFiltroLampada(Result.PontoEntregas[i].Img); }
                else if (TipoFiltro == "Status") { Result.PontoEntregas[i].Img = Result.PontoEntregas[i].Img; }
                else if (TipoFiltro == "NaSa") {
                    Result.Postes[i].Img = Result.PontoEntregas[i].Img;
                    if (Result.PontoEntregas[i].Img == "17") { quantNA++; }
                    else if (Result.PontoEntregas[i].Img == "19") { quantSA++; }
                }
            }
            RedePontoEntrega.push(new PontoEntregaMarker(Result.PontoEntregas[i], map));
        }
      /*  if (TipoFiltro == "NaSa") {
            MontaLegendaNaSa(quantNA, quantSA);
        }*/

        map.setZoom(14);
        if (Result.Centro.Lat && Result.Centro.Lon) { map.setCenter(new google.maps.LatLng(Result.Centro.Lat, Result.Centro.Lon)); }
        HideAllModal();
    }

    function CreateVaosDemandas(Result) {
        //if (Result.VaosDemandas.length == 0) { bootbox.alert("Não existe Vaos para esta consulta."); return; }
        var quantNA = 0;
        var quantSA = 0;
        for (var i = 0; i < Result.VaosDemandas.length; i++) {
            //caso um filtro esteja sendo aplicado os postes sao submetidos a esses metodos.
          /*  if (TipoFiltro != undefined && TipoFiltro != "") {
                if (TipoFiltro == "Potencia") { Result.VaosDemandas[i].Img = AtribuiImgFiltroPotencia(Result.VaosDemandas[i].Img); }
                else if (TipoFiltro == "Lampada") { Result.VaosDemandas[i].Img = AtribuiImgFiltroLampada(Result.VaosDemandas[i].Img); }
                else if (TipoFiltro == "Status") { Result.VaosDemandas[i].Img = Result.VaosDemandas[i].Img; }
                else if (TipoFiltro == "NaSa") {
                    Result.Postes[i].Img = Result.VaosDemandas[i].Img;
                    if (Result.VaosDemandas[i].Img == "17") { quantNA++; }
                    else if (Result.VaosDemandas[i].Img == "19") { quantSA++; }
                }
            }*/
            RedeVaosDemanda.push(new VaoDemandas(Result.VaosDemandas[i], map));
        }
        /*  if (TipoFiltro == "NaSa") {
              MontaLegendaNaSa(quantNA, quantSA);
          }*/

        map.setZoom(14);
        /*if (Result.Centro.Lat && Result.Centro.Lon) { map.setCenter(new google.maps.LatLng(Result.Centro.Lat, Result.Centro.Lon)); }
        HideAllModal();*/
    }

    //Criar os markers de Anotacoes
    function CreateAnotacao(retorno) {
      //  if (Result.VaosDemandas.length == 0) { bootbox.alert("Não existe Anotações para esta consulta."); return; }
        var quantNA = 0;
        var quantSA = 0;
        for (var i = 0; i < retorno.Result.length; i++) {
            //caso um filtro esteja sendo aplicado os postes sao submetidos a esses metodos.
            /*  if (TipoFiltro != undefined && TipoFiltro != "") {
                  if (TipoFiltro == "Potencia") { Result.VaosDemandas[i].Img = AtribuiImgFiltroPotencia(Result.VaosDemandas[i].Img); }
                  else if (TipoFiltro == "Lampada") { Result.VaosDemandas[i].Img = AtribuiImgFiltroLampada(Result.VaosDemandas[i].Img); }
                  else if (TipoFiltro == "Status") { Result.VaosDemandas[i].Img = Result.VaosDemandas[i].Img; }
                  else if (TipoFiltro == "NaSa") {
                      Result.Postes[i].Img = Result.VaosDemandas[i].Img;
                      if (Result.VaosDemandas[i].Img == "17") { quantNA++; }
                      else if (Result.VaosDemandas[i].Img == "19") { quantSA++; }
                  }
              }*/
            RedeAnotacao.push(new AnotacaoMarker(retorno.Result[i], map));
        }
        /*  if (TipoFiltro == "NaSa") {
              MontaLegendaNaSa(quantNA, quantSA);
          }*/

        map.setZoom(14);
        /*if (Result.Centro.Lat && Result.Centro.Lon) { map.setCenter(new google.maps.LatLng(Result.Centro.Lat, Result.Centro.Lon)); }
        HideAllModal();*/
    }
    //Criar os markers de Anotacoes
    function CreateStrands(retorno) {
        //  if (Result.VaosDemandas.length == 0) { bootbox.alert("Não existe Anotações para esta consulta."); return; }
        var quantNA = 0;
        var quantSA = 0;
        for (var i = 0; i < retorno.Result.length; i++) {
            //caso um filtro esteja sendo aplicado os postes sao submetidos a esses metodos.
            /*  if (TipoFiltro != undefined && TipoFiltro != "") {
                  if (TipoFiltro == "Potencia") { Result.VaosDemandas[i].Img = AtribuiImgFiltroPotencia(Result.VaosDemandas[i].Img); }
                  else if (TipoFiltro == "Lampada") { Result.VaosDemandas[i].Img = AtribuiImgFiltroLampada(Result.VaosDemandas[i].Img); }
                  else if (TipoFiltro == "Status") { Result.VaosDemandas[i].Img = Result.VaosDemandas[i].Img; }
                  else if (TipoFiltro == "NaSa") {
                      Result.Postes[i].Img = Result.VaosDemandas[i].Img;
                      if (Result.VaosDemandas[i].Img == "17") { quantNA++; }
                      else if (Result.VaosDemandas[i].Img == "19") { quantSA++; }
                  }
              }*/
            RedeStrands.push(new Strands(retorno.Result[i], map));
        }
        /*  if (TipoFiltro == "NaSa") {
              MontaLegendaNaSa(quantNA, quantSA);
          }*/

        map.setZoom(14);
        /*if (Result.Centro.Lat && Result.Centro.Lon) { map.setCenter(new google.maps.LatLng(Result.Centro.Lat, Result.Centro.Lon)); }
        HideAllModal();*/
    }
	//Inicia o mapa e funcionalidades do mesmo
    function LoadMap() {
        map = new google.maps.Map($('#globalMap').get(0), {
            center: new google.maps.LatLng(-22.906631919877885, -47.059739862060496), zoom: 15, zoomControl: true,
            mapTypeControl: true,
            mapTypeControlOptions: {
                style: google.maps.MapTypeControlStyle.LEFT_BOTTOM,
                position: google.maps.ControlPosition.LEFT_BOTTOM
            }
        });
        var input = (document.getElementById('cx_endereco'));
        var autocomplete = new google.maps.places.Autocomplete(input);
        autocomplete.bindTo('bounds', map);
        if ($.GetParamUrl('PosteByOs')) {
            PlotaPosteByOs();
        }
		else if (navigator.geolocation) {
		    navigator.geolocation.getCurrentPosition(function (position) {
		        map.setCenter({
		            lat: position.coords.latitude,
		            lng: position.coords.longitude
		        });
		    },
            function () { });
        }
    }
	
	//Limpa Postes do map e Array de poste
    function ClearPostes() { for (var i = 0; i < RedePostes.length; i++) { RedePostes[i].Remove(); } RedePostes = []; }
    function ClearArvores() { for (var i = 0; i < Arvores.length; i++) { Arvores[i].Remove(); } Arvores = []; }
    function ClearPontoEntrega() { for (var i = 0; i < RedePontoEntrega.length; i++) { RedePontoEntrega[i].Remove(); } RedePontoEntrega = []; }
    function ClearVaosDemandas() { for (var i = 0; i < RedeVaosDemanda.length; i++) { RedeVaosDemanda[i].Remove(); } RedeVaosDemanda = []; }

	//Metodo generio para usar ajax
    function CallServer(_type, _url, _param, _method) {
        Pace.start();
        $.ajax({
            type: _type, url: _url, cache: false,
            headers: { '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
            dataType: 'json', data: _param,
            success: function (d) { _method(d); },
            error: function (jqXHR, textStatus, errorThrown) {
                bootbox.alert($.parseJSON(jqXHR.responseText).Msg);
            }
        }).always(function () { Pace.stop(); });
    }

	//preenche lista de cidade do select no modal escolha cidade
    function PreencheHtmlDOMCidades() {
        CallServer("GET", "/AjaxRede/GetCidades", {}, function (d) {
            var $select = $('#selectCidade');
            $select.find('option').remove();
            $.each(d, function (key, value) { $select.append('<option value=' + value.IdCidade + '>' + value.Nome + '</option>'); });
            $('#ModalCidade').modal('show');
        });
    }
	
	//carrega novamente os postes com as informaçoes atuais da base de dados
    function UpdateRede() {
        if ($.GetParamUrl('PosteByOs')) {
            PlotaPosteByOs();
        }
        else {
            PlotaPostesByCidade();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////
    function PlotaQuadrasByCidade(idCidade) {
        CallServer("GET", "/Quadra/GetQuadrasByCidade", { IdCidade: idCidade },
        function (Result) {
            if (LimetesQuadras != null) {
                LimetesQuadras.Remove();
            }
            let quadras = Result.Result;
            LimetesQuadras = null;
            LimetesQuadras = new LimetesQuadrasModel(quadras, map);
        });
    }

    function PlotaQuadasByOs(ordemservico) {
        CallServer("GET", "/Quadra/GetQuadrasByOs", { OrdemServico: ordemservico },
        function (Result) {
            if (LimetesQuadras != null) {
                LimetesQuadras.Remove();
            }
            let quadras = Result.Result;
            LimetesQuadras = null;
            LimetesQuadras = new LimetesQuadrasModel(quadras, map);
        });
    }
    //////////////////////////////////////////////////////////////////////////////////////

	//plota os postes de uma cidade
    function PlotaPostesByCidade() {
        Id_Cidade = $("#selectCidade option:selected").val();
        if (Id_Cidade === undefined || Id_Cidade == "") return;
        ClearArvores();
        //ClearVaosDemandas();
        PlotaArvoresByCidade(Id_Cidade);
        PlotaQuadrasByCidade(Id_Cidade);
        CallServer("GET", "/AjaxRede/GetPostesByCidade", { idCidade: Id_Cidade },
            function (Result) {
                if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
                else {
                    window.history.pushState("", "Visium Geo - Index", window.location.href.split('?')[0]); // Alterando a URL HTML5
                    ClearPostes();
                    TipoFiltro = "";
					LimpaLegenda();
					Id_Cidade = Result.Postes.IdCidade;
                    $("#labelCidade").text("Cidade: " + Result.Postes.NomeCidade);
                    $("#labelOs").hide();
                    CreatePostes(Result.Postes);

                    if (LimiteCidade != null) {
                        LimiteCidade.Remove();
                    }
                    if (Result.Limites != null) {
                        LimiteCidade = null;
                        LimiteCidade = new Limetes(Result.Limites, map);
                    }
                }
            }
        );
    }

    function PlotaArvoresByCidade(idcidade) {
        CallServer("GET", "/Arvore/GetArvoresByCidade", { IdCidade: idcidade },
        function (Result) {
            if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
            else {
                if (Result.length > 0) {
                    $.each(Result, function (i, arvore_corrente) {
                        Arvores.push(new ArvoreMapa(arvore_corrente, map)); arvore_corrente
                    });
                }
            }
        });
    }

    function PlotaArvoresByOs(ordemservico) {
        CallServer("GET", "/Arvore/GetArvoresByOs", { OrdemServico: ordemservico },
        function (Result) {
            if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
            else {
                if (Result.length > 0) {
                    $.each(Result, function (i, arvore_corrente) {
                        Arvores.push(new ArvoreMapa(arvore_corrente, map)); arvore_corrente
                    });
                }
            }
        });
    }

    function PlotaPontosEntregasByOs(ordemservico) {
        CallServer("GET", "/PontodeEntrega/GetPontoEntregaByOs", { codOs: ordemservico },
        function (Result) {
            if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
            else {
                if (Result.length > 0) {
                    $.each(Result, function (i, ponto_entrega_corrente) {
                        RedePontoEntrega.push(new PontoEntregaMapa(ponto_entrega_corrente, map)); ponto_entrega_corrente
                    });
                }
            }
        });
    }

    function OpenModalArvore(idarvore) {
        CallServer("GET", "/Arvore/GetArvoresById", { IdArvore: idarvore },
        function (retorno) {
            if (retorno.Status != 2) { bootbox.alert("Erro"); return; }
            else {
                if (retorno.Result != null) {
                    var arvore = retorno.Result;
                    if (arvore.IdArvore != null) {
                        $("#input_id_arvore").val(arvore.IdArvore);
                    }
                    if (arvore.Porte != null) {
                        var porte_text = "Sem Informação"
                        switch (arvore.Porte) {
                            case 1:
                                porte_text = "PEQUENA";
                                break;
                            case 2:
                                porte_text = "MEDIA";
                                break;
                            case 3:
                                porte_text = "GRANDE";
                                break;
                        }
                        $("#input_porte_arvore").val(porte_text);
                    }

                    Lat_arvore = arvore.Latitude;
                    Lon_arvore = arvore.Longitude;

                    if (arvore.Fotos.length > 0) {
                        var content_indi = ""; content_inner = "";
                        $.each(arvore.Fotos, function (i, obj) {
                            content_indi += '<li data-target="#carouselarvore" data-slide-to="' + i + '"></li>';
                            content_inner += '<div class="item"><img src="' + '/FotoArvore/GetFoto?IdFoto=' + obj.IdFotoArvore + '&Width=600&Height=400" class="img-responsive" alt="..." onclick="javascript: window.open(&#39' + '/FotoArvore/GetFoto?IdFoto=' + obj.IdFotoArvore + '&Width=1024&Height=1024&#39, &#39_blank&#39);"> <div class="carousel-caption"><h4 class="text-danger"><b>' + obj.NumeroFoto + '</b></h4></div></div>';
                        });

                        $('#ol-carouselarvore').html(content_indi);
                        $('#div-carouselarvore').html(content_inner);
                        $('#div-carouselarvore .item').first().addClass('active');
                        $('#ol-carouselarvore > li').first().addClass('active');
                    }
                    else {
                        var content_indi = ""; content_inner = "";

                        content_indi += '<li data-target="#carouselarvore" data-slide-to="0"></li>';
                        content_inner += '<div class="item"><img src="' + '/FotoArvore/SemFoto" class="img-responsive"></div>';
                        $('#ol-carouselarvore').html(content_indi);
                        $('#div-carouselarvore').html(content_inner);
                        $('#div-carouselarvore .item').first().addClass('active');
                        $('#ol-carouselarvore > li').first().addClass('active');
                    }
                    $('#modalArvore').modal('show');
                }
            }
        });
    }
	
	//carrega Postes de uma OS
    function PlotaPosteByOs() {
        ClearPontoEntrega();
        ClearVaosDemandas();
        PlotaPontoEntregaByOs($.GetParamUrl('PosteByOs'));
        PlotaVaosDemandasByOs($.GetParamUrl('PosteByOs'));
        PlotaAnotacoesByOs($.GetParamUrl('PosteByOs'));
        PlotaStrandsByOs($.GetParamUrl('PosteByOs'));
        PlotaQuadasByOs($.GetParamUrl('PosteByOs'));
		CallServer("GET", "/AjaxRede/GetPostesByOs", { codOs: $.GetParamUrl('PosteByOs') },
		function (Result) {
			if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
			ClearPostes();
			TipoFiltro = "";
			LimpaLegenda();
			Id_Cidade = Result.Informacao.IdCidade;
			$("#labelCidade").text("Cidade: " + Result.Informacao.NomeCidade);
			$("#labelOs").show();
			$("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));
			if (Result.Postes != null) {
			    CreatePostes(Result.Postes);
			}
			

			if (LimiteCidade != null) {
			    LimiteCidade.Remove();
			}
			if (Result.Limites != null) {
			    LimiteCidade = null;
			    LimiteCidade = new Limetes(Result.Limites, map);
			}

			map.setZoom(14);
			if (Result.Centro.Lat && Result.Centro.Lon) { map.setCenter(new google.maps.LatLng(Result.Limites.LatitudeA, Result.Limites.LongitudeB)); }
		});
    }

    //carrega Ponto de Entrega de uma OS
    function PlotaPontoEntregaByOs(ordemServico) {
        ClearVaosDemandas();
        CallServer("GET", "/PontodeEntrega/GetPontoEntregaByOs", { codOs: ordemServico },
		function (Result) {
		    if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
		    ClearPontoEntrega();
		    TipoFiltro = "";
		    //LimpaLegenda();
		    //Id_Cidade = Result.Postes.IdCidade;
		    /*$("#labelCidade").text("Cidade: " + Result.Postes.NomeCidade);
		    $("#labelOs").show();
		    $("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));*/
		    CreatePontoEntrega(Result.PontoEntregas);

		    if (LimiteCidade != null) {
		        LimiteCidade.Remove();
		    }
		    if (Result.Limites != null) {
		        LimiteCidade = null;
		        LimiteCidade = new Limetes(Result.Limites, map);
		    }
		});
    }
	
    //carrega Postes de uma OS
    function PlotaVaosDemandasByOs(ordemServico) { 
        CallServer("GET", "/Vao/GetVaosDemandasByOs", { codOs: ordemServico },
		function (Result) {
		    if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
		   // ClearPontoEntrega();
		    TipoFiltro = "";
		    //LimpaLegenda();
		    //Id_Cidade = Result.Postes.IdCidade;
		    /*$("#labelCidade").text("Cidade: " + Result.Postes.NomeCidade);
		    $("#labelOs").show();
		    $("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));*/
		    CreateVaosDemandas(Result.VaosDemandas);

		    if (LimiteCidade != null) {
		        LimiteCidade.Remove();
		    }
		    if (Result.Limites != null) {
		        LimiteCidade = null;
		        LimiteCidade = new Limetes(Result.Limites, map);
		    }
		});
    }

    //carrega Postes de uma OS
    function PlotaAnotacoesByOs(ordemServico) { 
        CallServer("GET", "/Anotacoes/GetAnotacaoByOs", { codOs: ordemServico },
		function (Result) {
		    if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
		    // ClearPontoEntrega();
		    TipoFiltro = "";
		    //LimpaLegenda();
		    //Id_Cidade = Result.Postes.IdCidade;
		    /*$("#labelCidade").text("Cidade: " + Result.Postes.NomeCidade);
		    $("#labelOs").show();
		    $("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));*/
		    CreateAnotacao(Result);

		    /*if (LimiteCidade != null) {
		        LimiteCidade.Remove();
		    }
		    if (Result.Limites != null) {
		        LimiteCidade = null;
		        LimiteCidade = new Limetes(Result.Limites, map);
		    }*/
		});
    }

    //carrega Postes de uma OS
    function PlotaStrandsByOs(ordemServico) {
        CallServer("GET", "/Strands/GetStrandsByOs", { codOs: ordemServico },
		function (Result) {
		    if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
		    // ClearPontoEntrega();
		    TipoFiltro = "";
		    //LimpaLegenda();
		    //Id_Cidade = Result.Postes.IdCidade;
		    /*$("#labelCidade").text("Cidade: " + Result.Postes.NomeCidade);
		    $("#labelOs").show();
		    $("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));*/
		    CreateStrands(Result);

		    /*if (LimiteCidade != null) {
		        LimiteCidade.Remove();
		    }
		    if (Result.Limites != null) {
		        LimiteCidade = null;
		        LimiteCidade = new Limetes(Result.Limites, map);
		    }*/
		});
    }

	//carrega Postes com o filtro de potencia.
	function PlotaPostesPotencia() {
		if (Id_Cidade == undefined) { bootbox.alert("Não podemos proseguir, é necessario que escolha uma regiao!"); }
        else if ($.GetParamUrl('PosteByOs')) { PlotaPosteByOsPotencia(); }
		else { PlotaPostesByCidadePotencia(); }
    }
	
	//Plota os postes de uma Cidade com o filtro Potencia 
	function PlotaPostesByCidadePotencia() {
	    ClearArvores();
	    PlotaArvoresByCidade(Id_Cidade);
		CallServer("GET", "/AjaxRede/GetPostesByCidadePotencia", { idCidade: Id_Cidade },
			function (Result) {
				if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
				else {
					window.history.pushState("", "Visium Geo - Index", window.location.href.split('?')[0]); // Alterando a URL HTML5
					ClearPostes();
					TipoFiltro = "Potencia";
					//Limpa e Monta Legenda Potencia
					LimpaLegenda();
					MontaLegendaPotencia();
					Id_Cidade = Result.IdCidade;
					$("#labelCidade").text("Cidade: " + Result.NomeCidade);
					$("#labelOs").hide();
					CreatePostes(Result);
				}
			}
		);
	}
	
	//Plota os postes de uma OS com o filtro Potencia 
	function PlotaPosteByOsPotencia() {
	    ClearArvores();
	    PlotaArvoresByOs($.GetParamUrl('PosteByOs'));
		CallServer("GET", "/AjaxRede/GetPostesByOsPotencia", { codOs: $.GetParamUrl('PosteByOs') },
		function (Result) {
			if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
			ClearPostes();
			TipoFiltro = "Potencia";
			//Limpa e Monta Legenda Potencia
			LimpaLegenda();
			MontaLegendaPotencia();
			Id_Cidade = Result.IdCidade;
			$("#labelCidade").text("Cidade: " + Result.NomeCidade);
			$("#labelOs").show();
			$("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));
			CreatePostes(Result);
		});
	}
	
    //carrega Postes com o filtro de Status.
	function PlotaPostesStatus() {
		if (Id_Cidade == undefined) { bootbox.alert("Não podemos proseguir, é necessario que escolha uma regiao!"); }
        else if ($.GetParamUrl('PosteByOs')) { PlotaPosteByOsStatus(); }
		else { PlotaPostesByCidadeStatus(); }
    }
	
	//Plota os postes de uma Cidade com o filtro Status 
	function PlotaPostesByCidadeStatus() {
	    ClearArvores();
	    //PlotaArvoresByCidade(Id_Cidade);
		CallServer("GET", "/AjaxRede/GetPostesByCidadeStatus", { idCidade: Id_Cidade },
			function (Result) {
				if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
				else {
					window.history.pushState("", "Visium Geo - Index", window.location.href.split('?')[0]); // Alterando a URL HTML5
					ClearPostes();
					TipoFiltro = "Status";
					//Limpa e Monta Legenda Status
					LimpaLegenda();
					MontaLegendaStatus();
					Id_Cidade = Result.IdCidade;
					$("#labelCidade").text("Cidade: " + Result.NomeCidade);
					$("#labelOs").hide();
					CreatePostes(Result);
				}
			}
		);
	}
	
	//Plota os postes de uma OS com o filtro Status 
	function PlotaPosteByOsStatus() {
	    ClearArvores();
	    PlotaArvoresByOs($.GetParamUrl('PosteByOs'));
		CallServer("GET", "/AjaxRede/GetPostesByOsStatus", { codOs: $.GetParamUrl('PosteByOs') },
		function (Result) {
			if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
			ClearPostes();
			TipoFiltro = "Status";
			//Limpa e Monta Legenda Status
			LimpaLegenda();
			MontaLegendaStatus();
			Id_Cidade = Result.IdCidade;
			$("#labelCidade").text("Cidade: " + Result.NomeCidade);
			$("#labelOs").show();
			$("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));
			CreatePostes(Result);
		});
	}

    //legenda de status
	function MontaLegendaStatus() {
	    var conteudoLegenda = "";
	    conteudoLegenda += '<ul class="list-group">';
	    conteudoLegenda += '<li class="list-group-item"><img src="Images/17.png"> Sem Alteração</li>';
	    conteudoLegenda += '<li class="list-group-item"><img src="Images/19.png"> Adição de IP</li>';
	    conteudoLegenda += '<li class="list-group-item"><img src="Images/22.png"> Exclusão de IP</li>';
	    conteudoLegenda += '<li class="list-group-item"><img src="Images/23.png"> Alteração de IP</li>';
	    conteudoLegenda += '<li class="list-group-item"><img src="Images/13.png"> Sem Informação</li>';
	    conteudoLegenda += '</ul>';
	    $("#panelLegenda").append(conteudoLegenda);
	}

    //carrega Postes com o filtro de Status.
	function PlotaPostesNaSa() {
	    if (Id_Cidade == undefined) { bootbox.alert("Não podemos proseguir, é necessario que escolha uma regiao!"); }
	    else if ($.GetParamUrl('PosteByOs')) { PlotaPosteByOsNaSa(); }
	    else { PlotaPostesByCidadeNaSa(); }
	}

    //Plota os postes de uma Cidade com o filtro Status 
	function PlotaPostesByCidadeNaSa() {
	    ClearArvores();
	    PlotaArvoresByCidade(Id_Cidade);
	    CallServer("GET", "/AjaxRede/GetPostesByCidadeNaSa", { idCidade: Id_Cidade },
			function (Result) {
			    if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
			    else {
			        window.history.pushState("", "Visium Geo - Index", window.location.href.split('?')[0]); // Alterando a URL HTML5
			        ClearPostes();
			        TipoFiltro = "NaSa";
			        //Limpa e Monta Legenda Status
			        LimpaLegenda();
			        Id_Cidade = Result.IdCidade;
			        $("#labelCidade").text("Cidade: " + Result.NomeCidade);
			        $("#labelOs").hide();
			        CreatePostes(Result);
			    }
			}
		);
	}

    //Plota os postes de uma OS com o filtro Status 
	function PlotaPosteByOsNaSa() {
	    ClearArvores();
	    PlotaArvoresByOs($.GetParamUrl('PosteByOs'));
	    CallServer("GET", "/AjaxRede/GetPostesByOsNaSa", { codOs: $.GetParamUrl('PosteByOs') },
		function (Result) {
		    if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
		    ClearPostes();
		    TipoFiltro = "NaSa";
		    //Limpa e Monta Legenda Status
		    LimpaLegenda();
		    Id_Cidade = Result.IdCidade;
		    $("#labelCidade").text("Cidade: " + Result.NomeCidade);
		    $("#labelOs").show();
		    $("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));
		    CreatePostes(Result);
		});
	}

    //legenda de NaSa
	function MontaLegendaNaSa(quantNA, quantSA) {
	    var conteudoLegenda = "";
	    conteudoLegenda += '<ul class="list-group">';
	    conteudoLegenda += '<li class="list-group-item"><img src="Images/17.png"> Na - Quantidade ' + quantNA + '</li>';
	    conteudoLegenda += '<li class="list-group-item"><img src="Images/19.png"> Sa - Quantidade ' + quantSA + '</li>';
	    conteudoLegenda += '<li class="list-group-item"><img src="Images/13.png"> Sem Informação</li>';
	    conteudoLegenda += '</ul>';
	    $("#panelLegenda").append(conteudoLegenda);
	}

	//carrega Postes com o filtro de lampada.
	function PlotaPostesLampada() {
		if (Id_Cidade == undefined) { bootbox.alert("Não podemos proseguir, é necessario que escolha uma regiao!"); }
        else if ($.GetParamUrl('PosteByOs')) { PlotaPosteByOsLampada(); }
		else { PlotaPostesByCidadeLampada(); }
	}

	//Plota os postes de uma Cidade com o filtro Lampada
	function PlotaPostesByCidadeLampada() {
	    ClearArvores();
	    PlotaArvoresByCidade(Id_Cidade);
		CallServer("GET", "/AjaxRede/GetPostesByCidadeLampada", { idCidade: Id_Cidade },
			function (Result) {
				if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
				else {
					window.history.pushState("", "Visium Geo - Index", window.location.href.split('?')[0]); // Alterando a URL HTML5
					ClearPostes();
					TipoFiltro = "Lampada";
					//Limpa e Monta Legenda Lampada
					LimpaLegenda();
					MontaLegendaLampada();
					Id_Cidade = Result.IdCidade;
					$("#labelCidade").text("Cidade: " + Result.NomeCidade);
					$("#labelOs").hide();
					CreatePostes(Result);
				}
			}
		);
	}
	
	//Plota os postes de uma OS com o filtro Lampada 
	function PlotaPosteByOsLampada() {
	    ClearArvores();
	    PlotaArvoresByOs($.GetParamUrl('PosteByOs'));
		CallServer("GET", "/AjaxRede/GetPostesByOsLampada", { codOs: $.GetParamUrl('PosteByOs') },
		function (Result) {
			if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
			ClearPostes();
			TipoFiltro = "Lampada";
			//Limpa e Monta Legenda Lampada
			LimpaLegenda();
			MontaLegendaLampada();
			Id_Cidade = Result.IdCidade;
			$("#labelCidade").text("Cidade: " + Result.NomeCidade);
			$("#labelOs").show();
			$("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));
			CreatePostes(Result);
		});
	}

	//Abre Formulario com os dados do IP
    function EditaIluminacaoPublica(idIP) {
        //id poste cliado nesse momento
        var idPosteAssociado = $('#hiden_idPoste').attr("value");
        //limpa formulario de IP
        LimpaFormIP();
        //Limpa imagens que aparece Tela
        LimparImagens();
        //fecha todos os modal da pagina
        HideAllModal();
        //id padrao para add novo IP
        if (idIP == -1) {
            $('#idIluminacaoP').val(idIP);
            CallServer("GET", "/AjaxRede/GetIluminacaoPublicaById", { IdIP: idIP, IdPoste: idPosteAssociado },
			function (retorno) {
			    if (retorno.fotos.length > 0) {
			        //Preenche fotos associadas ao poste.
			        var content_indi = ""; content_inner = "";
			        $.each(retorno.fotos, function (i, obj) {
			            content_indi += '<li data-target="#carousel" data-slide-to="' + i + '"></li>';
			            content_inner += '<div class="item"><img src="' + '/Fotos/Poste?IdFoto=' + obj.IdFotoPoste + '&Width=600&Height=400" class="img-responsive" alt="..." onclick="javascript: window.open(&#39' + '/Fotos/Poste?IdFoto=' + obj.IdFotoPoste + '&Width=1024&Height=1024&#39, &#39_blank&#39);"><div class="carousel-caption"><h4 class="text-danger"><b>' + obj.NumeroFoto + '</b></h4></div></div>';
			        });
			        $('#ol-carousel').html(content_indi);
			        $('#div-carousel').html(content_inner);
			        $('#div-carousel .item').first().addClass('active');
			        $('#ol-carousel > li').first().addClass('active');
			    }
			    else {
			        var content_indi = ""; content_inner = "";
			        //caso nao tenha fotos add imagem indicando tal situação.
			        content_indi += '<li data-target="#carousel" data-slide-to="0"></li>';
			        content_inner += '<div class="item"><img src="' + '/Fotos/SemFoto" class="img-responsive"></div>';
			        $('#ol-carousel').html(content_indi);
			        $('#div-carousel').html(content_inner);
			        $('#div-carousel .item').first().addClass('active');
			        $('#ol-carousel > li').first().addClass('active');
			    }
			});
        }
        else {
            CallServer("GET", "/AjaxRede/GetIluminacaoPublicaById", { IdIP: idIP, IdPoste: idPosteAssociado },
			function (retorno) {
			    $('#idIluminacaoP').val(retorno.IPublica.IdIp);
			    $("#tipobraco").val(retorno.IPublica.TipoBraco);
			    $("#tipoluminaria").val(retorno.IPublica.TipoLuminaria);
			    $("#qtdluminaria").val(retorno.IPublica.QtdLuminaria);
			    $("#tipolampada").val(retorno.IPublica.TipoLampada);
			    $("#potencia").val(retorno.IPublica.Potencia);
			    if (retorno.IPublica.LampadaAcesa == "SIM" || retorno.IPublica.LampadaAcesa == "NAO") {
			        $("#lampadaacesa").val(retorno.IPublica.LampadaAcesa);
			    }
			    if (retorno.IPublica.Acionamento == "INDIVIDUAL" || retorno.IPublica.Acionamento == "GRUPO") {
			        $("#acionamento").val(retorno.IPublica.Acionamento);
			    }
			    $("#qtd_lampada").val(retorno.IPublica.QtdLampada);
			    $("#fase").val(retorno.IPublica.Fase);

			    if (retorno.fotos.length > 0) {
			        var content_indi = ""; content_inner = "";
			        $.each(retorno.fotos, function (i, obj) {
			            content_indi += '<li data-target="#carousel" data-slide-to="' + i + '"></li>';
			            content_inner += '<div class="item"><img src="' + '/Fotos/Poste?IdFoto=' + obj.IdFotoPoste + '&Width=600&Height=400" class="img-responsive" alt="..." onclick="javascript: window.open(&#39' + '/Fotos/Poste?IdFoto=' + obj.IdFotoPoste + '&Width=1024&Height=1024&#39, &#39_blank&#39);"><div class="carousel-caption"><h4 class="text-danger"><b>' + obj.NumeroFoto + '</b></h4></div></div>';
			        });
			        $('#ol-carousel').html(content_indi);
			        $('#div-carousel').html(content_inner);
			        $('#div-carousel .item').first().addClass('active');
			        $('#ol-carousel > li').first().addClass('active');
			    }
			    else {
			        var content_indi = ""; content_inner = "";

			        content_indi += '<li data-target="#carousel" data-slide-to="0"></li>';
			        content_inner += '<div class="item"><img src="' + '/Fotos/SemFoto" class="img-responsive"></div>';
			        $('#ol-carousel').html(content_indi);
			        $('#div-carousel').html(content_inner);
			        $('#div-carousel .item').first().addClass('active');
			        $('#ol-carousel > li').first().addClass('active');
			    }
			});
        }
        //Abre formulario de Edção de dados do IP
        $('#ModalEditIPFull').modal('show');
    }

	//Salva na Base as Ediçoes Feitas no IP Corrente
    function SalvarEditIP() {
        var idPosteAssociado = $('#hiden_idPoste').attr("value");
        CallServer("POST", "/AjaxRede/SalvarEdicaoIP", {
            _IdPosteAssociado: idPosteAssociado,
            _IdIp: $('#idIluminacaoP').val(),
            _TipoBraco: $("#tipobraco").val(),
            _TipoLuminaria: $("#tipoluminaria").val(),
            _TipoLampada: $("#tipolampada").val(),
            _QtdLuminaria: $("#qtdluminaria").val(),
            _Potencia: $("#potencia").val(),
            _Acionamento: $("#acionamento").val(),
            _LampadaAcesa: $("#lampadaacesa").val(),
            _Fase: $("#fase").val(),
            _QtdLampada: $("#qtd_lampada").val()
        },
		function (r) {
		    if (r.Status == 2) {
		        //facha todos os modal da pagina.
		        HideAllModal();
		        //caso esteja com um filtro, realiza o processamento necessario.
		        AplicaFiltro(idPosteAssociado);
		        //volta para menu de dados.
		        VoltarOpcoesPoste();
		        //modal de alert
		        bootbox.alert(r.Result);
		    }
		    else { bootbox.alert(r.Result); }
		});
    }

    //aplica filtro em apenas 1 poste por vez, logo apos salvar as
    //informaçoes na base, filtro é aplicado caso a pagina estiver com um filtro acionado
    function AplicaFiltro(Id_Poste) {
        if (TipoFiltro == "Lampada") {
            CallServer("GET", "/AjaxRede/GetPosteByIdFiltroLampada", { IdPoste: Id_Poste },
			function (poste) {
			    if ($('#VS').is(':checked') && poste.Img == "05") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
			    else if ($('#ME').is(':checked') && poste.Img == "06") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
			    else if ($('#VM').is(':checked') && poste.Img == "07") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
			    else if ($('#IN').is(':checked') && poste.Img == "09") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
			    else if ($('#MS').is(':checked') && poste.Img == "10") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
			    else if ($('#FL').is(':checked') && poste.Img == "11") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
			    else if ($('#SEMIP').is(':checked') && poste.Img == "04") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
			    //Tipo de Poste Nao Checado recebe esse tratamento.
				else { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/08.png'); } } }
			});
        }
        else if (TipoFiltro == "Potencia") {
		CallServer("GET", "/AjaxRede/GetPosteByIdFiltroPotencia", { IdPoste: Id_Poste },
			function (poste) {
				if ($('#Potencia_0').is(':checked') && poste.Img == "13") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_36').is(':checked') && poste.Img == "14") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_40').is(':checked') && poste.Img == "15") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_50').is(':checked') && poste.Img == "16") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_57').is(':checked') && poste.Img == "17") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_58').is(':checked') && poste.Img == "18") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_60').is(':checked') && poste.Img == "19") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_70').is(':checked') && poste.Img == "20") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_71').is(':checked') && poste.Img == "21") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_80').is(':checked') && poste.Img == "22") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_100').is(':checked') && poste.Img == "23") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_125').is(':checked') && poste.Img == "24") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_127').is(':checked') && poste.Img == "25") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_143').is(':checked') && poste.Img == "26") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_150').is(':checked') && poste.Img == "27") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_160').is(':checked') && poste.Img == "28") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_250').is(':checked') && poste.Img == "29") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else if ($('#Potencia_400').is(':checked') && poste.Img == "30") { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/' + poste.Img + '.png'); } } }
				else { for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == poste.IdPoste) { RedePostes[i].marker.setIcon('Images/12.png'); } } }
			});
        }
    }

	//Fecha os outros modais e Carrega o modal com as Opçoes do poste
    function VoltarOpcoesPoste() {
        //fecha todos os modal da pagina.
        HideAllModal();
        //abri o menu do poste com as opçoes refente ao mesmo.
        MontarBodyModalOpcoes();
    }

	//salva as ediçoes na base de dados realizados no poste.
    function SalvarEditPoste() {
        /// Processamento Foto

        var numero_foto, data_foto;
        var erro = false;

        for (var i = 0; i < identificadores_foto.length; i++)
        {
            numero_foto = $("#numero_foto_" + identificadores_foto[i]).val();
            data_foto = $("#data_foto_" + identificadores_foto[i]).val();

            if (numero_foto.length == 0 || data_foto.length == 0)
                erro = true;
        }

        if (!erro)
        {
            fotos_objetos = [];
            for (var i = 0; i < identificadores_foto.length; i++)
            {
                var foto;
                numero_foto = $("#numero_foto_" + identificadores_foto[i]).val();
                data_foto = $("#data_foto_" + identificadores_foto[i]).val();
                foto = { NumeroFoto: numero_foto, DataFoto: data_foto };
                fotos_objetos.push(foto);
            }
        }

        /// Variaveis dos Dados do Poste
        var Id_Poste = $('#hiden_idPoste').attr("value");
        var fotos = $("#numfotos").val();
        var altura = $("#altura_poste").val().toString().replace(/\./g, ',');
        var tipoposte = $("#tipo_poste").val();
        var esforco = $("#esforco_poste").val();
        var descricao = $("#descricao_poste").val();
        var data_diretotio = $("#data_diretotio_poste").val();
        var lat = $("#input_Latitude").val().replace(/\./g, ',');
        var lon = $("#input_Longitude").val().replace(/\./g, ',');
        
        var requestData = {
            IdCidade: Id_Cidade,
            IdPoste: Id_Poste,
            Latitude: lat,
            Longitude: lon,
            LstFoto: fotos_objetos,
            Altura: altura == "" ? 0 : altura,
            TipoPoste: tipoposte,
            Esforco: esforco,
            Descricao: descricao,
        };

        if (!erro) {
            $.ajax({
                url: '/AjaxRede/SalvarEdicaoPoste',
                type: 'POST',
                data: JSON.stringify(requestData),
                cache: false,
                headers: { '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
                dataType: 'json',
                contentType: 'application/json',
                error: function (xhr) {
                    bootbox.alert('Error: ' + xhr.statusText);
                    //abri o menu do poste com as opçoes refente ao mesmo.
                    VoltarOpcoesPoste();
                },
                success: function (r) {
                    if (r.Status == 2) {
                        bootbox.alert("Sucesso");
                        //abri o menu do poste com as opçoes refente ao mesmo.
                        VoltarOpcoesPoste();
                    } else { bootbox.alert("Erro : " + r.ErroMsg); }
                },
                async: true,
                processData: false
            });
        }
        else { bootbox.alert("Não podemos proseeguir! Campo Numero Foto ou Campo Data Foto Não Esta Preenchido !") }
    }

	//Carrega a table de Ip do Poste Clicado.
    function ListaIpByPoste() {
        //fecha todos os modal da pagina.
        HideAllModal();
        //remove as linha da table de ips do poste.
        RemoveAllLinhas();
        //poste clicado
        var idPosteAssociado = $('#hiden_idPoste').attr("value");
        CallServer("GET", "/AjaxRede/GetIluminacaoPublicaByPoste", { IdPoste: idPosteAssociado },
		function (lstIP) {
		    if (lstIP.length > 0) {
		        for (var i = 0; i < lstIP.length; i++) {
		            var tipobraco = lstIP[i].TipoBraco;
		            var tipoluminaria = lstIP[i].TipoLuminaria;
		            var qtdluminaria = lstIP[i].QtdLuminaria;
		            var tipolampada = lstIP[i].TipoLampada;
		            var potencia = lstIP[i].Potencia;
		            var idIP = lstIP[i].IdIp;
		            //add linhas na table de ips do poste.
		            AddLinha(tipobraco, tipoluminaria, qtdluminaria, tipolampada, potencia, idIP);
		        }
		        //abre a table com todos o ip do poste.
		        $('#ModalAllIpPoste').modal('show');
		    }
		    else{
		    	//abre a table com todos o ip do poste.
		        $('#ModalAllIpPoste').modal('show');
		    }
		});
    }

	//set o dh_eclusao do ip em questao 
    function ExcluirIp(idIP) {
        //bootbox confimação
        bootbox.confirm("<span class='glyphicon glyphicon-trash'></span> Desaja Realmente Excluir Esse IP ?", function (result) {
            if (result) {
                //fecha todos os madal da pagina.
                HideAllModal();
                CallServer("POST", "/AjaxRede/ExcluirIpById", { IdIp: idIP },
				function (r) {
				    if (r.Status == 2) { bootbox.alert(r.Result); }
				    else { bootbox.alert("Erro : " + r.Result); }
				});
            }
        });
    }

	//set o dh_exlusao na base e retira do mapa o seguinte poste
    function ExcluirPoste() {
        bootbox.confirm("<span class='glyphicon glyphicon-trash'></span> Deseja Realmente Exluir esse Poste ?", function (result) {
            if (result) {
                //fecha todos os madal da pagina.
                HideAllModal();
                //index do obj desejado na array
                var indexObj;
                //poste Clicado.
                var idPosteclicado = $('#hiden_idPoste').attr("value");
                for (var i = 0; i < RedePostes.length; i++) {
                    if (RedePostes[i].IdPoste == idPosteclicado) {
                        indexObj = i;
                    }
                }
                CallServer("POST", "/AjaxRede/ExcluirPoste", { IdPoste: idPosteclicado },
				function (r) {
				    if (r.Status == 2) {
				        RedePostes[indexObj].Remove();//remove do mapa
				        RedePostes.splice(indexObj, 1);//remove do array				        
				    }
				    bootbox.alert(r.Result);
				});
            }
        });
    }

    function ExcluirDemanda() {
        bootbox.confirm("<span class='glyphicon glyphicon-trash'></span> Deseja Realmente Exluir essa Demanda ?", function (result) {
            if(result){
                HideAllModal();

                var indiceArrayDemanda;
                var indiceArrayVao
                var idDemandaClicado = $("#hiden_idDemanda").attr("value");
                for (var i = 0; i < RedePontoEntrega.length; i++){
                    if (RedePontoEntrega[i].IdPontoEntrega == idDemandaClicado) {
                        indiceArrayDemanda = i;
                    }
                }

                for (var i = 0; i < RedeVaosDemanda.length; i++) {
                    if (RedeVaosDemanda[i].ID == idDemandaClicado) {
                        indiceArrayVao = i;
                    }
                }

                CallServer("POST", "/AjaxRede/ExcluirDemanda", { idDemanda: idDemandaClicado }, function (r) {
                    if(r.Status == 2){
                        RedePontoEntrega[indiceArrayDemanda].Remove();
                        RedePontoEntrega.splice[indiceArrayDemanda, 1];

                        RedeVaosDemanda[indiceArrayVao].Remove();
                        RedeVaosDemanda.splice[indiceArrayVao, 1];

                    }
                    bootbox.alert(r.Result);
                });
            }
        });
    }

	//Carrega um Marker Arrastavel para ser posicionado onde ficara o novo poste.
    function NovoPoste() {
        if (Id_Cidade == undefined) { bootbox.alert("Não podemos proseguir, é necessario que escolha uma regiao!"); }
        else {
            MakerAux = new google.maps.Marker({
                position: map.getCenter(),
                map: map,
                title: 'Posivel Novo Poste',
                draggable: true,
            });
            google.maps.event.addListener(MakerAux, 'click', function (event) {
                bootbox.confirm("<span class='glyphicon glyphicon-plus'></span> Deseja adicionar um poste nesse Local ?", function (result) {
                    if (result) { ConfirmaOsNovoPoste(); }
                    else { MakerAux.setMap(null); /*Remove esse Maker pois o mesmo nao sera usado*/ }
                });
            });
        }
    }

   /* function NovaDemanda() {
        if (Id_Cidade == undefined) { bootbox.alert("Não podemos proseguir, é necessario que escolha uma região") }
        else {
            MakerDemandaAux = new google.maps.Marker({
                position: map.getCenter(),
                map: map,
                title: 'posivel Nova Demanda',
                droggable: true,
            });
            google.maps.event.addListener(MakerDemandaAux, 'click', function (event) {
                bootbox.confirm("<span class='glyphicon glyphicon-plus'></span> Deseja adicionar uma Demanda nesse Local ?", function (result) {
                    if (result) {
                        ConfirmaOsNovaDemanda();
                    } else {
                        MakerDemandaAux.setMap(null);
                    }
                });
            });
        }
    }*/

	//o parametro OS nao pode ser nulo, para add um novo poste
    function ConfirmaOsNovoPoste() {
        //Preenche o Select de Ordem de Serviço
        CallServer("GET", "/AjaxRede/GetOsbyCidade", { idCidade: Id_Cidade },
		function (lstOs) {
		    if (lstOs.length > 0) {
		        for (var i = 0; i < lstOs.length; i++) {
		            $('#select-os').append('<option value="' + lstOs[i].IdOrdemDeServico + '">' + lstOs[i].NumeroOS + '</option>');
		        }
		    }
		    //add um Valor Vazio
		    $('#select-os').append('<option value="">Nao Informado</option>');

		    if ($.GetParamUrl('PosteByOs')) {
		        var os = $.GetParamUrl('PosteByOs')
		        $("#select-os option:contains(" + os + ")").attr('selected', 'selected');
		    }
		    else {
		        $('#select-os').val("");
		    }
		});
        //modal para a escolha da OS
        $('#ModalNewPoste').modal('show');
    }

    //o parametro OS nao pode ser nulo, para add um novo poste
    function ConfirmaOsNovaDemanda() {
        //Preenche o Select de Ordem de Serviço
        CallServer("GET", "/AjaxRede/GetOsbyCidade", { idCidade: Id_Cidade },
		function (lstOs) {
		    if (lstOs.length > 0) {
		        for (var i = 0; i < lstOs.length; i++) {
		            $('#select-os').append('<option value="' + lstOs[i].IdOrdemDeServico + '">' + lstOs[i].NumeroOS + '</option>');
		        }
		    }
		    //add um Valor Vazio
		    $('#select-os').append('<option value="">Nao Informado</option>');

		    if ($.GetParamUrl('PosteByOs')) {
		        var os = $.GetParamUrl('PosteByOs')
		        $("#select-os option:contains(" + os + ")").attr('selected', 'selected');
		    }
		    else {
		        $('#select-os').val("");
		    }
		});
        //modal para a escolha da OS
        $('#ModalNewPoste').modal('show');
    }


	//Salva um novo Poste no Base
    function SalvarNovoPoste() {
        var Os = $('#select-os').val();
        if (Os != "") {
            var requestData = {
                IdCidade: Id_Cidade,
                Latitude: MakerAux.getPosition().lat(),
                Longitude: MakerAux.getPosition().lng(),
                IdOrdemServico: Os,
            };

            $.ajax({
                url: '/AjaxRede/NewPoste',
                type: 'POST',
                data: JSON.stringify(requestData),
                cache: false,
                headers: { '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
                dataType: 'json',
                contentType: 'application/json',
                error: function (jqXHR, textStatus, errorThrown) {
                    bootbox.alert($.parseJSON(jqXHR.responseText).Msg);
                },
                success: function (r) {
                    if (r.Status == 1) {
                        //add o novo poste no mapra e no array 
                        RedePostes.push(new PosteMarker(r.Result, map));
                        NovoMaker.setMap(null);
                        //fecha todos os modal da pagina
                        HideAllModal();
                        bootbox.alert("Sucesso");
                    } else {
                        bootbox.alert(r.Result);
                    }
                },
                async: true,
                processData: false
            });
        }
        else { bootbox.alert("Nao Podemos Proseguir, Nao ha Ordem de Serviço Selecionada para o Novo Poste."); }
    }

	//Carrega novamente as informçoes do poste, que estao na base de dados.
    function CancelarEdicaoPoste() {
        //id poste clicado
        var idPosteclicado = $('#hiden_idPoste').attr("value");
        //index do obj no array
        var indexObj;
        //fecha todos os modal da pagina
        HideAllModal();
        for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == idPosteclicado) { indexObj = i; } }
        CallServer("GET", "/AjaxRede/GetPosteById", { IdPoste: idPosteclicado, IdCidade: Id_Cidade },
		function (retorno) {
		    //remove do mapa
		    RedePostes[indexObj].Remove();
		    //remove do array
		    RedePostes.splice(indexObj, 1);
		    //add o obj no array
		    RedePostes.push(new PosteMarker(retorno, map));
		});
    }

	//carraga  os Icon dos Makers de Acordo com o que foi Checado.
	function AtribuiImgFiltroLampada(Img){
		if ($('#VS').is(':checked') && Img == "05") { return Img; }
		else if ($('#ME').is(':checked') && Img == "06") { return Img; }
		else if ($('#VM').is(':checked') && Img == "07") { return Img; }
		else if ($('#IN').is(':checked') && Img == "09") { return Img; }
		else if ($('#MS').is(':checked') && Img == "10") { return Img; }
		else if ($('#FL').is(':checked') && Img == "11") { return Img; }
		else if ($('#SEMIP').is(':checked') && Img == "04") { return Img; }
		//Tipo de Poste Nao Checado recebe esse tratamento.
		else { return "08"; }
	}
	
	//carraga  os Icon dos Makers de Acordo com o que foi Checado.
	function AtribuiImgFiltroPotencia(Img){
		if ($('#Potencia_0').is(':checked') && Img == "13") { return Img; }
		else if ($('#Potencia_36').is(':checked') && Img == "14") { return Img; }
		else if ($('#Potencia_40').is(':checked') && Img == "15") { return Img; }
		else if ($('#Potencia_50').is(':checked') && Img == "16") { return Img; }
		else if ($('#Potencia_57').is(':checked') && Img == "17") { return Img; }
		else if ($('#Potencia_58').is(':checked') && Img == "18") { return Img; }
		else if ($('#Potencia_60').is(':checked') && Img == "19") { return Img; }
		else if ($('#Potencia_70').is(':checked') && Img == "20") { return Img; }
		else if ($('#Potencia_71').is(':checked') && Img == "21") { return Img; }
		else if ($('#Potencia_80').is(':checked') && Img == "22") { return Img; }
		else if ($('#Potencia_100').is(':checked') && Img == "23") { return Img; }
		else if ($('#Potencia_125').is(':checked') && Img == "24") { return Img; }
		else if ($('#Potencia_127').is(':checked') && Img == "25") { return Img; }
		else if ($('#Potencia_143').is(':checked') && Img == "26") { return Img; }
		else if ($('#Potencia_150').is(':checked') && Img == "27") { return Img; }
		else if ($('#Potencia_160').is(':checked') && Img == "28") { return Img; }
		else if ($('#Potencia_250').is(':checked') && Img == "29") { return Img; }
		else if ($('#Potencia_400').is(':checked') && Img == "30") { return Img; }
		else { return "12";}
	}
	
    //função para add linhas na table de ip para o poste.
    function AddLinha(tipobraco, tipoluminaria, qtdluminaria, tipolampada, potencia, idIp) {
        var newRow = '<tr>';
        newRow += '<td>' + idIp + '</td>';
        newRow += '<td>' + tipobraco + '</td>';
        newRow += '<td>' + tipoluminaria + '</td>';
        newRow += '<td>' + qtdluminaria + '</td>';
        newRow += '<td>' + tipolampada + '</td>';
        newRow += '<td>' + potencia + '</td>';
        newRow += '<td>Sem Informação</td>';
        newRow += '<td>';
        newRow += '<button title="Editar" class="btn btn-xs btn-warning" onclick="GlobalMaps.get().EditaIP(' + idIp + ');"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></button>&nbsp';
        newRow += '<button title="Excluir" class="btn btn-xs btn-danger" onclick="GlobalMaps.get().ConfirmaExclusao(' + idIp + ');"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></button>';
        newRow += '</td>';
        newRow += '</tr>';
        $("#tableIp").append(newRow);
    }

    //função para remover as linas da table de IP do Poste.
    function RemoveAllLinhas() {
        $("#tableIp").empty();
        var headTable = '<thead>';
        headTable += '<tr>';
        headTable += '<th class="info">Id IP</th>';
        headTable += '<th class="info">Tipo Braço</th>';
        headTable += '<th class="info">Tipo Luminaria</th>';
        headTable += '<th class="info">Qtd Luminaria</th>';
        headTable += '<th class="info">Tipo Lampada</th>';
        headTable += '<th class="info">Potencia</th>';
        headTable += '<th class="info">Rua</th>';
        headTable += '<th class="info"></th>';
        headTable += '</tr>';
        headTable += '</thead>';
        $("#tableIp").append(headTable);
    }

    //limpa as imagens do carousel de fotos.
    function LimparImagens() {
        $("#ol-carousel").empty();
        $("#div-carousel").empty();
    }

    //remove todos os botoes nao pertecente ao poste e add botoes Padrao. 
    function RemoveAllButtonModalOpcoes() {
    	//Remove os Botoes do Modal 
        $("#opcoesModal").empty();
    }

    //add os botoes pertencente ao poste.
    function MontarBodyModalOpcoes() {
        //remove todos os botoes e add os padroes.
        RemoveAllButtonModalOpcoes();
        //poste clicado.
        var idPosteclicado = $('#hiden_idPoste').attr("value");
        CallServer("GET", "/AjaxRede/ExisteIPbyPoste", { IdPoste: idPosteclicado },
		function (retorno) {
		    //verifica se o poste esta finalizado ou nao.
		    if (!retorno.Finalizado) {
		        $("#opcoesModal").append('<button type="button" onclick="GlobalMaps.get().FinalizarPoste();" class="btn btn-success btn-block"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span>&nbspFinalizar</button>');
		    } else {
		        $("#opcoesModal").append('<button type="button" onclick="GlobalMaps.get().CancelfinalizarPoste();" class="btn btn-danger btn-block"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span>&nbspReverter Finalizar</button>');
		    }
		});
        //menu de opçoes do poste.
        $('#ModalOpcoesPoste').modal('show');
    }

    //add os botoes pertencente ao poste.
    function MontarBodyModalPontoEntregaOpcoes() {
        //remove todos os botoes e add os padroes.
        RemoveAllButtonModalOpcoes();
        //poste clicado.
      /*  var idPosteclicado = $('#hiden_idPoste').attr("value");
        CallServer("GET", "/AjaxRede/ExisteIPbyPoste", { IdPoste: idPosteclicado },
		function (retorno) {
		    //verifica se o poste esta finalizado ou nao.
		    if (!retorno.Finalizado) {
		        $("#opcoesModal").append('<button type="button" onclick="GlobalMaps.get().FinalizarPoste();" class="btn btn-success btn-block"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span>&nbspFinalizar</button>');
		    } else {
		        $("#opcoesModal").append('<button type="button" onclick="GlobalMaps.get().CancelfinalizarPoste();" class="btn btn-danger btn-block"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span>&nbspReverter Finalizar</button>');
		    }
		});*/
        //menu de opçoes do poste.
        $('#ModalOpcoesPontoEntrega').modal('show');
    }

    //Limpa formulario do IP
    function LimpaFormIP() {
        $("#idIluminacaoP").val("");
        $("#tipobraco").val("");
        $("#tipoluminaria").val("");
        $("#qtdluminaria").val(0);
        $("#tipolampada").val("");
        $("#potencia").val(0);
        $("#lampadaacesa").val("");
        $("#acionamento").val("");
        $("#qtd_lampada").val(0);
        $("#fase").val("");
    }

    //Limpa Formulario do Poste
    function LimpaFormPoste() {
        $("#input_Latitude").val(0);
        $("#input_Longitude").val(0);
        $("#numfotos").val("");
        $("#altura_poste").val("");
        $("#tipo_poste").val(0);
        $("#esforco_poste").val("");
        $("#descricao_poste").val("");
        $("#data_diretotio_poste").val("");
        
    }

    //fecha todos os madal da pagina
    function HideAllModal() {
    	//fecha todos os modais pela classe
        $('.modal').modal('hide');
    }

    function EditarPoste() {
        //poste Clicado
        var idPosteclicado = $('#hiden_idPoste').attr("value");
        //numeros das fotos
        var numerofotos = "";
        //limpa formulario do poste
        LimpaFormPoste();
        //fecha todos os madal
        HideAllModal();
        /// Inicio Fotos
        cont_foto = 0;
        identificadores_foto = [];
        fotos_objetos = [];
        $("#div_fotos").empty();
        /// Fim Fotos

        CallServer("GET", "/AjaxRede/GetPosteById", { IdPoste: idPosteclicado },
            function (retorno) {
                $("#input_Latitude").val(retorno.Latitude);
                $("#input_Longitude").val(retorno.Longitude);
                $("#altura_poste").val(retorno.Altura);
                $("#tipo_poste").val(retorno.TipoPoste);
                $("#esforco_poste").val(retorno.Esforco);
                $("#descricao_poste").val(retorno.Descricao);
            });

        CallServer("GET", "/AjaxRede/GetFotosByPoste", { IdPoste: idPosteclicado },
			function (fotos) {
			    if (fotos.length > 0) {
			        var content_indi = ""; content_inner = "";
			        $.each(fotos, function (i, obj) {
			            content_indi += '<li data-target="#carouselposte" data-slide-to="' + i + '"></li>';
			            content_inner += '<div class="item"><img src="' + '/Fotos/Poste?IdFoto=' + obj.IdFotoPoste + '&Width=600&Height=400" class="img-responsive" alt="..." onclick="javascript: window.open(&#39' + '/Fotos/Poste?IdFoto=' + obj.IdFotoPoste + '&Width=1024&Height=1024&#39, &#39_blank&#39);"> <div class="carousel-caption"><h4 class="text-danger"><b>' + obj.NumeroFoto + '</b></h4></div></div>';

			            /// FOTOS

			            cont_foto++;
			            var identificador_foto = 'foto_' + cont_foto;

			            var conteudo_foto = '<div class="input-group" id="' + identificador_foto + '">';
			            conteudo_foto += '<span class="input-group-addon">DATA FOTO</span>';
			            conteudo_foto += '<input id="data_foto_' + identificador_foto + '" type="date" class="form-control" />';
			            conteudo_foto += '<span class="input-group-addon">NUMERO FOTO</span>';
			            conteudo_foto += '<input id="numero_foto_' + identificador_foto + '" type="text" class="form-control" maxlength="10"/>';
			            conteudo_foto += '<span class="input-group-btn"><button class="btn btn-danger" type="button" onclick="GlobalMaps.get().RemoveFoto(' + identificador_foto + ');">EXCLUIR</button></span>';
			            conteudo_foto += '</div>';
			            conteudo_foto += '<p></p>';

			            $("#div_fotos").append(conteudo_foto);

			            identificadores_foto.push(identificador_foto);

			            $("#numero_foto_" + identificador_foto).val(obj.NumeroFoto);
			            $("#data_foto_" + identificador_foto).val(obj.DataFoto);
			        });
			        $('#ol-carouselposte').html(content_indi);
			        $('#div-carouselposte').html(content_inner);
			        $('#div-carouselposte .item').first().addClass('active');
			        $('#ol-carouselposte > li').first().addClass('active');
			    }
			    else {
			        var content_indi = ""; content_inner = "";

			        content_indi += '<li data-target="#carouselposte" data-slide-to="0"></li>';
			        content_inner += '<div class="item"><img src="' + '/Fotos/SemFoto" class="img-responsive"></div>';
			        $('#ol-carouselposte').html(content_indi);
			        $('#div-carouselposte').html(content_inner);
			        $('#div-carouselposte .item').first().addClass('active');
			        $('#ol-carouselposte > li').first().addClass('active');
			    }
			});
        
        //monta formulario do poste.	
        $("#ModalEditPoste").modal('show');
    }

    function EditarPontoEntrega() {
        //poste Clicado
        var idPontoEntrega = $('#hiden_idDemanda').attr("value");
        //numeros das fotos
        var numerofotos = "";
        //limpa formulario do poste
        LimpaFormPoste();
        //fecha todos os madal
        HideAllModal();
        /// Inicio Fotos
        cont_foto = 0;
        identificadores_foto = [];
        fotos_objetos = [];
        $("#div_foto_ponto_entrega").empty();
        /// Fim Fotos

        CallServer("GET", "/AjaxRede/GetPontoEntregaById", { idPontoEntrega: idPontoEntrega },
            function (retorno) {
                $("#latitude_ponto_entrega").val(retorno.Latitude);
                $("#longitude_ponto_entrega").val(retorno.Longitude);
                $("#numero_ponto_entrega").val(retorno.Numero);
                $("#codigo_geo_ponto_entrega").val(retorno.CodigoGeoBD);
                $("#classe_social_ponto_entrega").val(retorno.ClasseSocial);
                $("#complemento1_ponto_entrega").val(retorno.Complemento1);
                $("#complemento2_ponto_entrega").val(retorno.Complemento2);
                //$("#descricao_poste").val(retorno.Descricao);
            });

        CallServer("GET", "/AjaxRede/GetFotosByPontoEntrega", { idPontoEntrega: idPontoEntrega },
			function (fotos) {
			    if (fotos.length > 0) {
			        var content_indi = ""; content_inner = "";
			        $.each(fotos, function (i, obj) {
			            content_indi += '<li data-target="#carouselposte" data-slide-to="' + i + '"></li>';
			            content_inner += '<div class="item"><img src="' + '/Fotos/Poste?IdFoto=' + obj.IdFotoPontoEntrega + '&Width=600&Height=400" class="img-responsive" alt="..." onclick="javascript: window.open(&#39' + '/Fotos/Poste?IdFoto=' + obj.IdFotoPontoEntrega + '&Width=1024&Height=1024&#39, &#39_blank&#39);"> <div class="carousel-caption"><h4 class="text-danger"><b>' + obj.NumeroFoto + '</b></h4></div></div>';

			            /// FOTOS

			            cont_foto++;
			            var identificador_foto = 'foto_' + cont_foto;

			            var conteudo_foto = '<div class="input-group" id="' + identificador_foto + '">';
			            conteudo_foto += '<span class="input-group-addon">DATA FOTO</span>';
			            conteudo_foto += '<input id="data_foto_' + identificador_foto + '" type="date" class="form-control" />';
			            conteudo_foto += '<span class="input-group-addon">NUMERO FOTO</span>';
			            conteudo_foto += '<input id="numero_foto_' + identificador_foto + '" type="text" class="form-control" maxlength="10"/>';
			            conteudo_foto += '<span class="input-group-btn"><button class="btn btn-danger" type="button" onclick="GlobalMaps.get().RemoveFoto(' + identificador_foto + ');">EXCLUIR</button></span>';
			            conteudo_foto += '</div>';
			            conteudo_foto += '<p></p>';

			            $("#div_foto_ponto_entrega").append(conteudo_foto);

			            identificadores_foto.push(identificador_foto);

			            $("#numero_foto_" + identificador_foto).val(obj.NumeroFoto);
			            $("#data_foto_" + identificador_foto).val(obj.DataFoto);
			        });
			        $('#ol-carouselpontoentrega').html(content_indi);
			        $('#div-carouselpontoentrega').html(content_inner);
			        $('#div-carouselpontoentrega .item').first().addClass('active');
			        $('#ol-carouselpontoentrega > li').first().addClass('active');
			    }
			    else {
			        var content_indi = ""; content_inner = "";

			        content_indi += '<li data-target="#carouselposte" data-slide-to="0"></li>';
			        content_inner += '<div class="item"><img src="' + '/Fotos/SemFoto" class="img-responsive"></div>';
			        $('#ol-carouselpontoentrega').html(content_indi);
			        $('#div-carouselpontoentrega').html(content_inner);
			        $('#div-carouselpontoentrega .item').first().addClass('active');
			        $('#ol-carouselpontoentrega > li').first().addClass('active');
			    }
			});

        //monta formulario do poste.	
        $("#ModalEditPontoEntrega").modal('show');
    }

    function MoveUpPoste() {
        var lat = $("#input_Latitude").val();
        var x = new Number(lat);
        x += 0.00005;
        $("#input_Latitude").val(x);
        var idPosteclicado = $('#hiden_idPoste').attr("value");
        for (var i = 0; i < RedePostes.length; i++) {
            if (RedePostes[i].IdPoste == idPosteclicado) {
                RedePostes[i].Latitude = x;
                RedePostes[i].marker.setPosition(new google.maps.LatLng(RedePostes[i].Latitude, RedePostes[i].Longitude));
            }
        }
    }

    function MoveDownPoste() {
        var lat = $("#input_Latitude").val();
        var x = new Number(lat);
        x -= 0.00005;
        $("#input_Latitude").val(x);
        var idPosteclicado = $('#hiden_idPoste').attr("value");
        for (var i = 0; i < RedePostes.length; i++) {
            if (RedePostes[i].IdPoste == idPosteclicado) {
                RedePostes[i].Latitude = x;
                RedePostes[i].marker.setPosition(new google.maps.LatLng(RedePostes[i].Latitude, RedePostes[i].Longitude));
            }
        }
    }

    function MoveLeftPoste() {
        var lat = $("#input_Longitude").val();
        var x = new Number(lat);
        x -= 0.00005;
        $("#input_Longitude").val(x);
        var idPosteclicado = $('#hiden_idPoste').attr("value");
        for (var i = 0; i < RedePostes.length; i++) {
            if (RedePostes[i].IdPoste == idPosteclicado) {
                RedePostes[i].Longitude = x;
                RedePostes[i].marker.setPosition(new google.maps.LatLng(RedePostes[i].Latitude, RedePostes[i].Longitude));
            }
        }
    }

    function MoveRightPoste() {
        var lat = $("#input_Longitude").val();
        var x = new Number(lat);
        x += 0.00005;
        $("#input_Longitude").val(x);
        var idPosteclicado = $('#hiden_idPoste').attr("value");
        for (var i = 0; i < RedePostes.length; i++) {
            if (RedePostes[i].IdPoste == idPosteclicado) {
                RedePostes[i].Longitude = x;
                RedePostes[i].marker.setPosition(new google.maps.LatLng(RedePostes[i].Latitude, RedePostes[i].Longitude));
            }
        }
    }

	//Filtro para Ip de Lampada
    function EscolhaLampada() {
        //fecha todos os modal da pagina.
        HideAllModal();
        //fitro de lampada
        $('#ModalFiltroLampada').modal('show');
    }

	//Filtro para Ip de Potencia
    function EscolhaPotencia() {
        //fecha todos os modal da pagina.
        HideAllModal();
        //fitro de lampada
        $('#ModalFiltroPotencia').modal('show');
    }

	//escolha dos possiveis filtros para IP
    function OpcoesdeFiltroIP() {
        //fecha todos os modal da pagina.
        HideAllModal();
        //opçoes de filtro IP
        $('#opcoes_Ip').modal('show');
    }

	//Metodo para parar de aplicar um filtro de busca ex: lampada, potencia.
    function SemFiltro() {
        if (Id_Cidade == undefined) {
            bootbox.alert("Não podemos proseguir, é necessario que escolha uma regiao!");
        }
        else if ($.GetParamUrl('PosteByOs')) {
            PlotaPosteByOs();
        }
        else {
            PlotaPostesByCidade();
        }
    }

    function LimpaLegenda() {
        $('#panelLegenda').empty();
    }

    function MontaLegendaLampada() {
        var conteudoLegenda = "";
        conteudoLegenda += '<ul class="list-group">';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/05.png">VS</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/06.png">ME</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/07.png">VM</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/09.png">IN</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/10.png">MS</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/11.png">FL</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/04.png">SEM IP</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/08.png">SEM FILTRO</li>';
        conteudoLegenda += '</ul>';
        $("#panelLegenda").append(conteudoLegenda);
    }

    function MontaLegendaPotencia() {
        var conteudoLegenda = "";
        conteudoLegenda += '<ul class="list-group">';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/13.png">Potencia 0 / <img src="Images/14.png">Potencia 36</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/15.png">Potencia 40 / <img src="Images/16.png">Potencia 50</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/17.png">Potencia 57 / <img src="Images/18.png">Potencia 58</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/19.png">Potencia 60 / <img src="Images/20.png">Potencia 70</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/21.png">Potencia 71 / <img src="Images/22.png">Potencia 80</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/23.png">Potencia 100 / <img src="Images/24.png">Potencia 125</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/25.png">Potencia 127 / <img src="Images/26.png">Potencia 143</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/27.png">Potencia 150 / <img src="Images/28.png">Potencia 160</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/29.png">Potencia 250 / <img src="Images/30.png">Potencia 400</li>';
        conteudoLegenda += '<li class="list-group-item"><img src="Images/12.png">SEM FILTRO</li>';
        conteudoLegenda += '</ul>';
        $("#panelLegenda").append(conteudoLegenda);
    }

	//Buscado de poste por codigo Geo
    function BuscaPoste() {
        bootbox.prompt("Qual o ID do Poste que procura?", function (result) {
            if (result === null) {
                /*Nada Aconte, user clicou em Cancel*/
             } else {
                var CodPoste = result;
                var achou = false;
                for (var i = 0; i < RedePostes.length; i++) {
                    if (RedePostes[i].CodGeo == CodPoste) {
                        achou = true;
                        RedePostes[i].marker.setIcon('Images/01.png');
                        map.setCenter(new google.maps.LatLng(RedePostes[i].Latitude, RedePostes[i].Longitude));
                        map.setZoom(18);
                    }
                }
                if (!achou) {
                    bootbox.alert("Nao Encontramos um Poste com esse ID!");
                }
            }
        });
    }

	//Buscado de poste por codigo Numero
    function BuscaPosteByNumero() {
    	var num_ordem = $.GetParamUrl('PosteByOs');
    	if(num_ordem != null){
	        bootbox.prompt("Qual o Numero do Poste que procura?", function (result) {
	            if (result === null) {
	                /*Nada Aconte, user clicou em Cancel*/
	            } else {
	                var NumeroPoste = result;
	                var achou = false;
			        $.each(RedePostes, function (i, PosteCorrente) {
						if(PosteCorrente.NumeroPosteNaOS == NumeroPoste){
		                    achou = true;
		                    PosteCorrente.marker.setIcon('Images/01.png');
		                    map.setCenter(new google.maps.LatLng(PosteCorrente.Latitude, PosteCorrente.Longitude));
		                    map.setZoom(18);
						}
					});
	                if (!achou) {
	                    bootbox.alert("Nao Encontramos um Poste com esse Numero");
	                }
	            }
	        });
    	}
    	else{
    		bootbox.alert("Funcão exclusiva para Ordem de Serviço.");
    	}

    }

	//set o status do poste para finalizado
    function FinalizarPoste() {
        var Id_Poste = $('#hiden_idPoste').attr("value");
        CallServer("POST", "/AjaxRede/FinalizarPoste", { IdPoste: Id_Poste },
		function (r) {
		    if (r.Status == 2) {
		        bootbox.alert(r.Result);
		        for (var i = 0; i < RedePostes.length; i++) {
		            if (RedePostes[i].IdPoste == Id_Poste) {
		                RedePostes[i].marker.setIcon('Images/10.png');
		            }
		        }
		    }
		    else {
		        bootbox.alert(r.Result);
		    }
		});
        //fecha todos os modal da pagina.
        HideAllModal();
    }

	//caso o usuario tenha setado o status para finalizado, pode reverter com essa função. 
    function CancelfinalizarPoste() {
        var Id_Poste = $('#hiden_idPoste').attr("value");
        CallServer("POST", "/AjaxRede/CancelfinalizarPoste", { IdPoste: Id_Poste },
		function (r) {
		    if (r.Status == 2) {
		        bootbox.alert(r.Result);
		        for (var i = 0; i < RedePostes.length; i++) {
		            if (RedePostes[i].IdPoste == Id_Poste) {
		                RedePostes[i].marker.setIcon('Images/08.png');
		            }
		        }
		    }
		    else {
		        bootbox.alert(r.Result);
		    }
		});
        //fecha todos os modal da pagina.
        HideAllModal();
    }

	//Buscador de Endereço no mapa
    function BuscaEndereco() {
        var endereco = $('#cx_endereco').val();
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': endereco }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                map.setCenter(results[0].geometry.location);
                map.setZoom(18);
            } else {
                bootbox.alert('Falha ' + status);
            }
        });
    }

	//Abre uma Nova Aba com o StreetView no local do poste clicado.
    function AbreStreetView() {
        var Id_Poste = $('#hiden_idPoste').attr("value");
        var Lat, Lon;
        for (var i = 0; i < RedePostes.length; i++) {
            if (RedePostes[i].IdPoste == Id_Poste) {
                Lat = RedePostes[i].Latitude;
                Lon = RedePostes[i].Longitude;
            }
        }
        window.open('/Rede/StreetView?Latitude=' + Lat + '&Longitude=' + Lon + '', '_blank');
    }

    function AbreStreetViewPontoEntrega() {
        var idPontoEntrega = $('#hiden_idDemanda').attr("value");
        var Lat, Lon;
        for (var i = 0; i < RedePontoEntrega.length; i++) {
            if (RedePontoEntrega[i].IdPontoEntrega == idPontoEntrega) {
                Lat = RedePontoEntrega[i].Latitude;
                Lon = RedePontoEntrega[i].Longitude;
            }
        }
        window.open('/Rede/StreetView?Latitude=' + Lat + '&Longitude=' + Lon + '', '_blank');
    }

    //Abre uma Nova Aba com o StreetView no local do poste clicado.
    function AbreStreetViewArvore() {
        window.open('/Rede/StreetView?Latitude=' + Lat_arvore + '&Longitude=' + Lon_arvore + '', '_blank');
    }

    function AddFoto() {

        cont_foto++;

        var identificador_foto = 'foto_' + cont_foto;

        var conteudo_foto = '<div class="input-group" id="'+ identificador_foto +'">';
        conteudo_foto += '<span class="input-group-addon">DATA FOTO</span>';
        conteudo_foto += '<input id="data_foto_' + identificador_foto + '" type="date" class="form-control" />';
        conteudo_foto += '<span class="input-group-addon">NUMERO FOTO</span>';
        conteudo_foto += '<input id="numero_foto_' + identificador_foto + '" type="text" class="form-control" maxlength="10"/>';
        conteudo_foto += '<span class="input-group-btn"><button class="btn btn-danger" type="button" onclick="GlobalMaps.get().RemoveFoto(' + identificador_foto + ');">EXCLUIR</button></span>';
        conteudo_foto += '</div>';
        conteudo_foto += '<p></p>';

        $("#div_fotos").append(conteudo_foto);

        identificadores_foto.push(identificador_foto);
    }

    function RemoveFoto(identificador) {
        var indexObj;
        for (var i = 0; i < identificadores_foto.length; i++)
        {
            if (identificadores_foto[i] == identificador.id)
            {
                indexObj = i;
            }
        }
        identificadores_foto.splice(indexObj, 1);//remove do array				        
        $(identificador).remove();
    }

    ///////////////////////////////////////////////////PONTO DE ENTREGA///////////////////////////////////////////////////
    
    //Carrega a table de Ponto de Entrega do Poste Clicado.
    function ListaPontoEntregaByPoste(){
        //fecha todos os modal da pagina.
        HideAllModal();
        //remove as linha da table de Pontos de Entrega do poste.
        RemoveAllLinhasPontoEntrega();
        //Chamada para o servidor
        CallServer("GET", "/PontodeEntrega/GetListaPontodeEntrega", { IdPoste: $('#hiden_idPoste').attr("value") },
		function (ListaPontoEntrega) {
		    if (ListaPontoEntrega.length > 0) {
		        for (var i = 0; i < ListaPontoEntrega.length; i++) {
		        	//Trabalha com o ponto de entrega corrento
		        	var ponto_corrente = ListaPontoEntrega[i];

		            //add linhas na table de Pontos Entregas do poste.
		            AddLinhaPontoEntrega(
		            	ponto_corrente.IdPontoEntrega,
		            	ponto_corrente.CodigoGeoBD,
		            	ponto_corrente.QuantidadeMedidores
		            	);
		        }
		        //abre a table com todos o ip do poste.
		        $('#ModalAllPontoEntregaPoste').modal('show');
		    }
		    else{
		    	//abre a table com todos o ip do poste.
		        $('#ModalAllPontoEntregaPoste').modal('show');
		    }
		});
    }

    //função para add linhas na table de PontoEntrega para o poste.
    function AddLinhaPontoEntrega(idpontoentrega, codigobdgeo, qtdmedidores) {
        var newRow = '<tr>';
        newRow += '<td>' + idpontoentrega + '</td>';
        newRow += '<td>' + codigobdgeo + '</td>';
        newRow += '<td>' + qtdmedidores + '</td>';
        newRow += '<td>';
        newRow += '<button title="Editar" class="btn btn-xs btn-warning" onclick="GlobalMaps.get().EditarPontoEntrega(' + idpontoentrega + ');"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></button>&nbsp';
        newRow += '<button title="Excluir" class="btn btn-xs btn-danger" onclick="GlobalMaps.get().ConfirmaExclusao(' + idpontoentrega + ');"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></button>';
        newRow += '</td>';
        newRow += '</tr>';
        $("#tablePontodeEntrega").append(newRow);
    }

    //função para remover as linas da table de IP do Poste.
    function RemoveAllLinhasPontoEntrega() {
        $("#tablePontodeEntrega").empty();
        var headTable = '<thead>';
        headTable += '<tr>';
        headTable += '<th class="info">Id Ponto Entrega</th>';
        headTable += '<th class="info">Código Geo</th>';
        headTable += '<th class="info">Quantidade Medidores</th>';
        headTable += '<th class="info"></th>';
        headTable += '</tr>';
        headTable += '</thead>';
        $("#tablePontodeEntrega").append(headTable);
    }

    function LimpaFormPontoEntrega(){
		$("#id_ponto_entrega").val("");
		$("#id_poste_ponto_entrega").val("");
		$("#codigo_geo_ponto_entrega").val("");
		$("#latitude_ponto_entrega").val("");
		$("#longitude_ponto_entrega").val("");
		$("#numero_ponto_entrega").val("");
		$("#logradouro_ponto_entrega").val("");
		$("#fase_ponto_entrega").val("");
		$("#et_ligacao_ponto_entrega").val("");
		$("#status_ponto_entrega").val(0);
		$("#classe_atendimento_ponto_entrega").val(0);
		$("#tipo_construcao_ponto_entrega").val(0);
		$("#classe_social_ponto_entrega").val(0);
		$("#observacao_ponto_entrega").val("");
		
		/// Inicio Fotos
        cont_foto_ponto = 0;
        identificadores_foto_ponto = [];
        fotos_objetos_ponto = [];
        $("#div_foto_ponto_entrega").empty();
        /// Fim Fotos
    }

    function LimpaDivMedidores(){
        contador_medidor = 0;
        $("#div_medidores").empty();
        medidores_objetos = [];
    }

   /* function EditarPontoEntrega(idpontodeentrega) {
        //fecha todos os madal
        HideAllModal();
        //limpa formulario do poste
        LimpaFormPontoEntrega();

        //Pega Dados do Ponto de Entrega
        CallServer("GET", "/PontodeEntrega/GetPontodeEntrega", { IdPontoEntrega: idpontodeentrega },
			function (ponto_corrente) {
				$("#id_ponto_entrega").val(ponto_corrente.IdPontoEntrega);
				$("#id_poste_ponto_entrega").val(ponto_corrente.IdPoste);
				$("#codigo_geo_ponto_entrega").val(ponto_corrente.CodigoGeoBD);
				$("#latitude_ponto_entrega").val(ponto_corrente.Latitude);
				$("#longitude_ponto_entrega").val(ponto_corrente.Longitude);
				$("#numero_ponto_entrega").val(ponto_corrente.Numero);
				$("#logradouro_ponto_entrega").val(ponto_corrente.Logradouro);
				$("#fase_ponto_entrega").val(ponto_corrente.Fase);
				$("#et_ligacao_ponto_entrega").val(ponto_corrente.EtLigacao);
				$("#status_ponto_entrega").val(ponto_corrente.Status);
				$("#classe_atendimento_ponto_entrega").val(ponto_corrente.ClasseAtendimento);
				$("#tipo_construcao_ponto_entrega").val(ponto_corrente.TipoConstrucao);
				$("#classe_social_ponto_entrega").val(ponto_corrente.ClasseSocial);
				$("#observacao_ponto_entrega").val(ponto_corrente.Observacao);
			});

        LimpaDivMedidores();
        //Pega Dados do Medidores
        CallServer("GET", "/PontodeEntrega/GetMedidores", { IdPontoEntrega: idpontodeentrega },
			function (medidores) {
		        $.each(medidores, function (i, medidor) {
		            //contador para controlar os medidores a tela
		            contador_medidor++;
		            //id unico no javascript do medidor
		            var local_id_medidor = 'medidor_' + contador_medidor;
					//montando uma div para o medidor
		            var div_medidor = '';
		            div_medidor += '<div class="input-group" id="div_' + local_id_medidor + '">';
		            div_medidor += '<span class="input-group-addon">NUMERO</span>';
		            div_medidor += '<input id="numero_' + local_id_medidor + '" type="text" class="form-control" maxlength="20" value="'+ medidor.NumeroMedidor +'" />';
		            div_medidor += '<span class="input-group-addon">COMPLEMENTO</span>';
		            div_medidor += '<input id="complemento_' + local_id_medidor + '" type="text" class="form-control" maxlength="30" value="'+ medidor.ComplementoResidencial +'" />';
		            div_medidor += '<span class="input-group-btn"><button class="btn btn-danger" type="button" onclick="GlobalMaps.get().RemoveMedidor(\''+local_id_medidor+'\')">EXCLUIR</button></span>';
		            div_medidor += '</div>';
		            div_medidor += '<p></p>';
		            //adicionando o div medidoe na div medidores
		            $("#div_medidores").append(div_medidor);
		            //
		            var medidor_objeto = { identificador : local_id_medidor, };
		            //
		            medidores_objetos.push(medidor_objeto);
		        });
			});

        CallServer("GET", "/FotoPontoEntrega/GetFotosByPontoEntrega", { IdPontoEntrega: idpontodeentrega },
			function (fotos) {
			    if (fotos.length > 0) {
			        var content_indi = ""; content_inner = "";
			        $.each(fotos, function (i, obj) {
			            content_indi += '<li data-target="#carouselpontoentrega" data-slide-to="' + i + '"></li>';
			            content_inner += '<div class="item"><img src="' + '/FotoPontoEntrega/GetFoto?IdFoto=' + obj.IdFotoPontoEntrega + '&Width=600&Height=400" class="img-responsive" alt="..." onclick="javascript: window.open(&#39' + '/FotoPontoEntrega/GetFoto?IdFoto=' + obj.IdFotoPontoEntrega + '&Width=1024&Height=1024&#39, &#39_blank&#39);"> <div class="carousel-caption"><h4 class="text-danger"><b>' + obj.NumeroFoto + '</b></h4></div></div>';
			        
						/// FOTOS

			            cont_foto_ponto++;
			            var identificador_foto_ponto = 'foto_' + cont_foto_ponto;

			            var conteudo_foto_ponto = '<div class="input-group" id="' + identificador_foto_ponto + '">';
			            conteudo_foto_ponto += '<span class="input-group-addon">DATA FOTO</span>';
			            conteudo_foto_ponto += '<input id="data_foto_' + identificador_foto_ponto + '" type="date" class="form-control" />';
			            conteudo_foto_ponto += '<span class="input-group-addon">NUMERO FOTO</span>';
			            conteudo_foto_ponto += '<input id="numero_foto_' + identificador_foto_ponto + '" type="text" class="form-control" maxlength="10"/>';
			            conteudo_foto_ponto += '<span class="input-group-btn"><button class="btn btn-danger" type="button" onclick="GlobalMaps.get().RemoveFotoPontoEntrega(' + identificador_foto_ponto + ');">EXCLUIR</button></span>';
			            conteudo_foto_ponto += '</div>';
			            conteudo_foto_ponto += '<p></p>';

			            $("#div_foto_ponto_entrega").append(conteudo_foto_ponto);

			            identificadores_foto_ponto.push(identificador_foto_ponto);

			            $("#numero_foto_" + identificador_foto_ponto).val(obj.NumeroFoto);
			            $("#data_foto_" + identificador_foto_ponto).val(obj.DataFoto);
					
					});

			        $('#ol-carouselpontoentrega').html(content_indi);
			        $('#div-carouselpontoentrega').html(content_inner);
			        $('#div-carouselpontoentrega .item').first().addClass('active');
			        $('#ol-carouselpontoentrega > li').first().addClass('active');
			    }
			    else {
			        var content_indi = ""; content_inner = "";

			        content_indi += '<li data-target="#carouselpontoentrega" data-slide-to="0"></li>';
			        content_inner += '<div class="item"><img src="' + '/FotoPontoEntrega/SemFoto" class="img-responsive"></div>';
			        $('#ol-carouselpontoentrega').html(content_indi);
			        $('#div-carouselpontoentrega').html(content_inner);
			        $('#div-carouselpontoentrega .item').first().addClass('active');
			        $('#ol-carouselpontoentrega > li').first().addClass('active');
			    }
			});

        //monta formulario do poste.
        $("#ModalEditPontoEntrega").modal('show');
    }*/
	
	function AddFotoPontoEntrega() {

        cont_foto_ponto++;

        var identificador_foto_ponto = 'foto_' + cont_foto_ponto;

        var conteudo_foto_ponto = '<div class="input-group" id="'+ identificador_foto_ponto +'">';
        conteudo_foto_ponto += '<span class="input-group-addon">DATA FOTO</span>';
        conteudo_foto_ponto += '<input id="data_foto_' + identificador_foto_ponto + '" type="date" class="form-control" />';
        conteudo_foto_ponto += '<span class="input-group-addon">NUMERO FOTO</span>';
        conteudo_foto_ponto += '<input id="numero_foto_' + identificador_foto_ponto + '" type="text" class="form-control" maxlength="10"/>';
        conteudo_foto_ponto += '<span class="input-group-btn"><button class="btn btn-danger" type="button" onclick="GlobalMaps.get().RemoveFotoPontoEntrega(' + identificador_foto_ponto + ');">EXCLUIR</button></span>';
        conteudo_foto_ponto += '</div>';
        conteudo_foto_ponto += '<p></p>';

        $("#div_foto_ponto_entrega").append(conteudo_foto_ponto);

        identificadores_foto_ponto.push(identificador_foto_ponto);
    }
	
	function RemoveFotoPontoEntrega(identificador) {
        var indexObj;
        for (var i = 0; i < identificadores_foto_ponto.length; i++)
        {
            if (identificadores_foto_ponto[i] == identificador.id)
            {
                indexObj = i;
            }
        }
        identificadores_foto_ponto.splice(indexObj, 1);//remove do array				        
        $(identificador).remove();
    }

    function EditarNovoPontoEntrega() {
        //fecha todos os madal
        HideAllModal();
        //limpa formulario do poste
        LimpaFormPontoEntrega();

		$("#id_ponto_entrega").val(-1);
		$("#id_poste_ponto_entrega").val($('#hiden_idPoste').attr("value"));
		$("#codigo_geo_ponto_entrega").val(0);
		$("#latitude_ponto_entrega").val(0);
		$("#longitude_ponto_entrega").val(0);

        LimpaDivMedidores();

		var content_indi = ""; content_inner = "";

		content_indi += '<li data-target="#carouselpontoentrega" data-slide-to="0"></li>';
		content_inner += '<div class="item"><img src="' + '/FotoPontoEntrega/SemFoto" class="img-responsive"></div>';
		$('#ol-carouselpontoentrega').html(content_indi);
		$('#div-carouselpontoentrega').html(content_inner);
		$('#div-carouselpontoentrega .item').first().addClass('active');
		$('#ol-carouselpontoentrega > li').first().addClass('active');

        //monta formulario do poste.
        $("#ModalEditPontoEntrega").modal('show');
    }

    function AddMedidor(){
	    //contador para controlar os medidores a tela
	    contador_medidor++;
	    //id unico no javascript do medidor
	    var local_id_medidor = 'medidor_' + contador_medidor;
		//montando uma div para o medidor
	    var div_medidor = '';
	    div_medidor += '<div class="input-group" id="div_' + local_id_medidor + '">';
	    div_medidor += '<span class="input-group-addon">NUMERO</span>';
	    div_medidor += '<input id="numero_' + local_id_medidor + '" type="text" class="form-control" maxlength="20" />';
	    div_medidor += '<span class="input-group-addon">COMPLEMENTO</span>';
	    div_medidor += '<input id="complemento_' + local_id_medidor + '" type="text" class="form-control" maxlength="30" />';
	    div_medidor += '<span class="input-group-btn"><button class="btn btn-danger" type="button" onclick="GlobalMaps.get().RemoveMedidor(\''+local_id_medidor+'\')">EXCLUIR</button></span>';
	    div_medidor += '</div>';
	    div_medidor += '<p></p>';
	    //adicionando o div medidoe na div medidores
	    $("#div_medidores").append(div_medidor);
	    //
	    var medidor_objeto = { identificador : local_id_medidor, };
	    //
	    medidores_objetos.push(medidor_objeto);
    }

    function RemoveMedidor(identificador){
    	//Guarda o index o item do arry a ser excluido.
    	var index_obj;
    	//percorre todo o array
    	for (var i = 0; i < medidores_objetos.length; i++) {
    		if(medidores_objetos[i].identificador == identificador){
    			$("#div_" + identificador).remove();
    			index_obj = i;
    		}
    	}
		//remove do array
		if (index_obj === undefined){
		//so para garantir dexei assim
		}else{
			medidores_objetos.splice(index_obj, 1);	
		}
    }

	//salva as ediçoes na base de dados realizados no Ponto de Entrega
    function SalvarEdicaoPontoEntrega() {
    //	var medidores = [];
		var fotos = [];
    	var erro = false;
    	//dados do ponto de entrega
    	var ponto_entrega = {
            IdPontoEntrega : $("#id_ponto_entrega").val(),
            CodigoGeoBD : $("#codigo_geo_ponto_entrega").val(),
            ClasseAtendimento : $("#classe_atendimento_ponto_entrega").val(),    	    
            Numero : $("#numero_ponto_entrega").val(),
            ClasseSocial : $("#classe_social_ponto_entrega").val(),
            Logradouro : $("#logradouro_ponto_entrega").val(),
            Observacao : $("#observacao_ponto_entrega").val(),
            Latitude : $("#latitude_ponto_entrega").val().replace(/\./g, ','),
            Longitude : $("#longitude_ponto_entrega").val().replace(/\./g, ','),
            QuantidadeMedidores : 0,
            Fase : $("#fase_ponto_entrega").val(),    	    
            Fotos: fotos

    	    //   Medidores : medidores,
    	    //    EtLigacao : $("#et_ligacao_ponto_entrega").val(),
    	    //  TipoConstrucao : $("#tipo_construcao_ponto_entrega").val(),
    	    //  Status : $("#status_ponto_entrega").val(),
    	    // IdPoste : $("#id_poste_ponto_entrega").val(),
    	};
    	//percorre todo o array
    /*	for (var i = 0; i < medidores_objetos.length; i++) {
    		var numero_corrente = $("#numero_" + medidores_objetos[i].identificador).val();
    		var complemento_corrente = $("#complemento_" + medidores_objetos[i].identificador).val();

    		if(numero_corrente.length > 0 && complemento_corrente.length > 0){
    			var medidor = {
	    			IdMedidor : -1,
	    			IdPontoEntrega : ponto_entrega.IdPontoEntrega,
	    			NumeroMedidor : numero_corrente,
	    			ComplementoResidencial : complemento_corrente,
    			};

    			medidores.push(medidor);
    		}
    		else{
    			erro = true;
    		}
    	}*/
		
		/// Processamento Foto

        var numero_foto, data_foto;

        for (var i = 0; i < identificadores_foto_ponto.length; i++)
        {
            numero_foto = $("#numero_foto_" + identificadores_foto_ponto[i]).val();
            data_foto = $("#data_foto_" + identificadores_foto_ponto[i]).val();

            if (numero_foto.length == 0 || data_foto.length == 0)
                erro = true;
        }

        if (!erro)
        {
            for (var i = 0; i < identificadores_foto_ponto.length; i++)
            {
                var foto;
                numero_foto = $("#numero_foto_" + identificadores_foto_ponto[i]).val();
                data_foto = $("#data_foto_" + identificadores_foto_ponto[i]).val();
                foto = { NumeroFoto: numero_foto, DataFoto: data_foto };
                fotos.push(foto);
            }
        }
		else { bootbox.alert("Não podemos prosseguir! Campo Numero Foto ou Data Foto não esta preenchido !") }

        if (!erro) {
        	//ponto_entrega.Medidores = medidores;
        	//ponto_entrega.QuantidadeMedidores = medidores.length;
			ponto_entrega.Fotos = fotos;

	        CallServer("POST", "/PontodeEntrega/SalvarPontoEntrega", { PontoEntregaReceived : ponto_entrega },
				function (response) {
					VoltarOpcoesPoste();
					bootbox.alert(response.Result);
				});
        }
        else { bootbox.alert("Não podemos prosseguir! Campo Numero ou Complemento não esta preenchido !") }
    }  
    

    function VisualizarPontosEntregaMapa(){
    	var num_ordem = $.GetParamUrl('PosteByOs');

		if(num_ordem != null){
	        CallServer("GET", "/PontodeEntrega/PontoEntregaCoordenadaByOrdem", { NumeroOrdem : num_ordem },
				function (lstCoordenadas) {
					if(lstCoordenadas.length > 0){
						ApagarPontosEntregaMapa();
						RedePontoEntrega = [];
				        $.each(lstCoordenadas, function (i, Coordenadas) {
							RedePontoEntrega.push(new PontoEntregaMapa(Coordenadas,map));
						});
				    }else{
				    	bootbox.alert("Não ha Pontos de Entregas para essa Cidade/Ordem de Serviço.");
				    }
				});
		}
    	else if(Id_Cidade === undefined){
    		bootbox.alert("Uma cidede deve estar selecionada!");
    	}
    	else{
	        CallServer("GET", "/PontodeEntrega/PontoEntregaCoordenada", { IdCidade : Id_Cidade },
				function (lstCoordenadas) {
					if(lstCoordenadas.length > 0){
						ApagarPontosEntregaMapa();
						RedePontoEntrega = [];
				        $.each(lstCoordenadas, function (i, Coordenadas) {
							RedePontoEntrega.push(new PontoEntregaMapa(Coordenadas,map));
						});
				    }else{
				    	bootbox.alert("Não ha Pontos de Entregas para essa Cidade/Ordem de Serviço.");
				    }
				});
    	}
    }

    function ApagarPontosEntregaMapa(){
        $.each(RedePontoEntrega, function (i, PontosEntregaMapa) {
			PontosEntregaMapa.Remove();
		});
    }

    function VisualizarVaosMapa(){
    	var num_ordem = $.GetParamUrl('PosteByOs');

		if(num_ordem != null){
	        CallServer("GET", "/Vao/GetVaosByOs", { NumeroOrdem : num_ordem },
				function (lstCoordenadas) {
					if(lstCoordenadas.length > 0){
						ApagarVaosMapa();
						RedeVaos = [];
				        $.each(lstCoordenadas, function (i, CoordenadasVao) {
							RedeVaos.push(new VaoMapa(CoordenadasVao,map));
						});
					}else{
						bootbox.alert("Não ha Vãos para essa Cidade/Ordem de Serviço.");
					}
				});
		}
		else if(Id_Cidade === undefined){
			bootbox.alert("Uma cidede deve estar selecionada!");
    	}
    	else{
	        CallServer("GET", "/Vao/GetVaosByCidade", { IdCidade : Id_Cidade },
				function (lstCoordenadas) {
					if(lstCoordenadas.length > 0){
						ApagarVaosMapa();
						RedeVaos = [];
				        $.each(lstCoordenadas, function (i, CoordenadasVao) {
							RedeVaos.push(new VaoMapa(CoordenadasVao,map));
						});
					}else{
						bootbox.alert("Não ha Vãos para essa Cidade/Ordem de Serviço.");
					}
				});
    	}
    }

    function ApagarVaosMapa(){
        $.each(RedeVaos, function (i, VaoCorrente) {
			VaoCorrente.Remove();
		});
    }

    function MoveRede(somaLatitude, somaLongitude) {
        var latPoste = 0;
        var lonPoste = 0;
        var latArvore = 0;
        var lonArvore = 0;

        for (var i = 0; i < RedePostes.length; i++) {

            latPoste = RedePostes[i].marker.position.lat();
            lonPoste = RedePostes[i].marker.position.lng();

            latPoste += somaLatitude;
            lonPoste += somaLongitude;

            RedePostes[i].marker.setPosition(new google.maps.LatLng(latPoste, lonPoste));
        }
        for (var i = 0; i < Arvores.length; i++) {
            latArvore = Arvores[i].marker.position.lat();
            lonArvore = Arvores[i].marker.position.lng();

            latArvore += somaLatitude;
            lonArvore += somaLongitude;

            Arvores[i].marker.setPosition(new google.maps.LatLng(latArvore, lonArvore));
        }
        //for (var i = 0; i < RedePontoEntrega.length; i++) { RedePontoEntrega[i].setPosition(RedePontoEntrega[i].marker.position.latitude + somaLatitude, RedePontoEntrega[i].marker.position.longitude + somaLongitude); }
        //for (var i = 0; i < RedeVaos.length; i++) { RedeVaos[i].setPosition(RedeVaos[i].marker.position.latitude + somaLatitude, RedeVaos[i].marker.position.longitude + somaLongitude); }
    }

    function init() {
        if (typeof (jQuery) === 'undefined') { alert("O jQuery não foi carregado. Verifique o import da libs."); return; }
        if (!(typeof google === 'object' && typeof google.maps === 'object')) { alert("O GoogleMaps API não foi carregado. Verifique o import da libs."); return; }
        return {
            StartMap: function () { LoadMap(); },
            ModalEscolheCidade: function () { PreencheHtmlDOMCidades(); },
            PlotaPostesByCidade: function () { PlotaPostesByCidade(); },
            EditaIP: function (idIP) { EditaIluminacaoPublica(idIP); },
            SalvarIP: function () { SalvarEditIP(); },
            SalvarEdicaoPoste: function () { SalvarEditPoste(); },
            ListaIpByPoste: function () { ListaIpByPoste(); },
            MontarModalOpcoes: function () { MontarBodyModalOpcoes(); },
            MontarModalPontoOpcoes: function () { MontarBodyModalPontoEntregaOpcoes(); },
            ConfirmaExclusao: function (idIP) { ExcluirIp(idIP); },
            NovoPoste: function () { NovoPoste(); },
            EditarPoste: function () { EditarPoste(); },
            MovePosteCima: function () { MoveUpPoste(); },
            MovePosteBaixo: function () { MoveDownPoste(); },
            MovePosteDireita: function () { MoveRightPoste(); },
            MovePosteEsquerda: function () { MoveLeftPoste(); },
            CancelEditPoste: function () { CancelarEdicaoPoste(); },
            ExcluirPoste: function () { ExcluirPoste(); },
            EscolhaLampada: function () { EscolhaLampada(); },
            EscolhaPotencia: function () { EscolhaPotencia(); },
            SemFiltro: function () { SemFiltro(); },
            BuscaPoste: function () { BuscaPoste(); },
            SalvarNovoPoste: function () { SalvarNovoPoste(); },
            FinalizarPoste: function () { FinalizarPoste(); },
            CancelfinalizarPoste: function () { CancelfinalizarPoste(); },
            VoltarOpcoesPoste: function () { VoltarOpcoesPoste(); },
            BuscaEndereco: function () { BuscaEndereco(); },
            UpdateRede: function () { UpdateRede(); },
            OpcoesdeFiltroIP: function () { OpcoesdeFiltroIP(); },
            AbreStreetView: function () { AbreStreetView(); },
			PlotaPostesPotencia: function () { PlotaPostesPotencia(); },
			PlotaPostesStatus: function () { PlotaPostesStatus(); },
			PlotaPostesLampada: function () { PlotaPostesLampada(); },
			PlotaPostesNaSa: function () { PlotaPostesNaSa(); },
			GetPostesPorCidadeEPagina: function (page) { GetPostesPorCidadeEPagina(page); },
			AddFoto: function () { AddFoto(); },
			RemoveFoto: function (identificador) { RemoveFoto(identificador); },
			ListaPontoEntregaByPoste: function () { ListaPontoEntregaByPoste(); },
			EditarPontoEntrega: function (idpontodeentrega) { EditarPontoEntrega(idpontodeentrega); },
			RemoveMedidor: function (identificador) { RemoveMedidor(identificador); },
			AddMedidor: function () { AddMedidor(); },
			SalvarEdicaoPontoEntrega: function () { SalvarEdicaoPontoEntrega(); },
			EditarNovoPontoEntrega: function () { EditarNovoPontoEntrega(); },
			VisualizarPontosEntregaMapa: function () { VisualizarPontosEntregaMapa(); },
			ApagarPontosEntregaMapa: function () { ApagarPontosEntregaMapa(); },
			VisualizarVaosMapa: function () { VisualizarVaosMapa(); },
			ApagarVaosMapa: function () { ApagarVaosMapa(); },
			BuscaPosteByNumero: function () { BuscaPosteByNumero(); },
			AddFotoPontoEntrega: function () { AddFotoPontoEntrega(); },
			RemoveFotoPontoEntrega: function (identificador) { RemoveFotoPontoEntrega(identificador); },
			OpenModalArvore: function (idarvore) { OpenModalArvore(idarvore); },
			AbreStreetViewArvore: function () { AbreStreetViewArvore(); },
			MoveRede: function (somaLatitude, somaLongitude) { MoveRede(somaLatitude, somaLongitude) },
			AbreStreetViewPontoEntrega: function () { AbreStreetViewPontoEntrega() },
			ExcluirDemanda: function () { ExcluirDemanda ()}
			//NovaDemanda: function () { NovaDemanda() }
        };
    };

    return { get: function () { if (!instance) { instance = init(); } return instance; } };
})();
    GlobalMaps.get().StartMap();