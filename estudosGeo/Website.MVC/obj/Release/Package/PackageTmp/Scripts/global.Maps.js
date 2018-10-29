var criarDemanda = false;
var criarStrand = false;
var latPoste, lonPoste, idPosteClickado;
var latPosteAntigo, lonPosteAntigo;
var idPosteClickado1;
var markerArrastado;

var numDeltas = 100;
var delay = 10; //milliseconds
var c = 0;
var deltaLat;
var deltaLng;
var positionMarkerUser;



function MapLabelsHome(item, map) {
    /*this.mapLabel = new MapLabel({
        text: item.NumOS + " / " + item.Colaborador,
        position: item.Bound.getCenter(),
        map: map,
        fontSize: 13,
        align: 'center',
        fontColor: "#FFFFFF",
        strokeColor: "#515151",
        strokeWeight: 5,
        zIndex: 15
    });
    */
    this.markerLabel = new RichMarker({
        map: map,
        shadow: 'none',
        position: item.Bound.getCenter(),
        content: '<div><div class="label_content">' + item.NumOS + "-" + item.Colaborador// the data title you want to display
        + '</div></div>'
    });

    this.Remove = function () { this.markerLabel.setMap(null) }
}

function LocalUserHome(item, map) {
    /*this.mapLabel = new MapLabel({
        text: item.NumOS + " / " + item.Colaborador,
        position: item.Bound.getCenter(),
        map: map,
        fontSize: 13,
        align: 'center',
        fontColor: "#FFFFFF",
        strokeColor: "#515151",
        strokeWeight: 5,
        zIndex: 15
    });
    */
    this.marker = new RichMarker({
        map: map,
        shadow: 'none',        
        position: new google.maps.LatLng(item.X, item.Y),
        animation: google.maps.Animation.DROP,        
        content: '<div><div class="label_content_localUser">' + item.NomeUser// the data title you want to display
        + '</div></div>'
    });

    this.markerIcon = new google.maps.Marker({
        position: new google.maps.LatLng(item.X, item.Y),
        map: map,
        title: 'Ultimo Acesso: ' + item.Time,
        icon: 'Images/' + "person" + '.png'
    });

   
    this.NomeUser = item.NomeUser;
    this.Remove = function () { this.mapLabel.setMap(null) }
}

function PosteMarkerHome(p, map) {
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
        icon: 'Images/' + "postehome" + '.png'
    });
    this.marker.setMap(map);
    var CodigoGeo = p.CodGeo;
    //  google.maps.event.addListener(this.marker, 'click', function (event) { $('#lb_IdPoste').text(CodigoGeo); $('#hiden_idPoste').val(NumeroPoste); $('#hiden_codGeo').val(CodigoGeo); GlobalMaps.get().MontarModalOpcoes(); });
    this.Remove = function () { this.marker.setMap(null) }
}

function DemandaMarkerHome(p, map) {
    this.IdPontoEntrega = p.IdPontoEntrega;
    this.Latitude = p.Latitude;
    this.Longitude = p.Longitude;
    this.NomeImagem = p.Img;
    this.ClasseSocial = p.ClasseSocial;
    this.Numero = p.Numero;
    var NumeroPoste = p.IdPoste;
    this.marker = new google.maps.Marker({
        position: new google.maps.LatLng(this.Latitude, this.Longitude),
        map: map,
        title: 'Tipo da Demanda ' + this.ClasseSocial + ' / Número: ' + this.Numero,
        icon: 'Images/' + "demandahome" + '.png'
    });
    this.marker.setMap(map);
    var CodigoGeo = p.CodGeo;
    //  google.maps.event.addListener(this.marker, 'click', function (event) { $('#lb_IdPoste').text(CodigoGeo); $('#hiden_idPoste').val(NumeroPoste); $('#hiden_codGeo').val(CodigoGeo); GlobalMaps.get().MontarModalOpcoes(); });
    this.Remove = function () { this.marker.setMap(null) }
}

function StrandsHome(coordenadas, map) {

    var positionLine = [{ lat: (coordenadas.X1), lng: (coordenadas.Y1) }, { lat: (coordenadas.X2), lng: (coordenadas.Y2) }];
    this.IdDemandaStrand = coordenadas.IdDemandaStrand;
    this.X1 = coordenadas.X1;
    this.Y1 = coordenadas.Y1;
    this.line = new google.maps.Polyline({
        path: positionLine,
        geodesic: true,
        strokeColor: '#ffa100',
        strokeOpacity: 1.0,
        strokeWeight: 4,
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
        strokeColor: '#ffa100',
        strokeOpacity: 1.0,
        strokeWeight: 4,
        map: map
    });

    var idPoly = coordenadas.ID;

    google.maps.event.addListener(this.line, 'click', function () {
        $('#hiden_idStrands').val(idPoly); GlobalMaps.get().MontarModalStrands();
    });

    this.Remove = function () {
        this.line.setMap(null);
    };
}

function VaoDemandasHome(coordenadas, map) {

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

function AnotacaoMarkerOpcoes(p, map) {
    this.IdAnotacao = p.IdAnotacao;
    this.X = p.X;
    this.Y = p.Y;
    this.Descricao = p.Descricao,
    this.CodGeo = p.CodGeo;
    this.marker = new google.maps.Marker({
        position: new google.maps.LatLng(this.X, this.Y),
        map: map,
        animation: google.maps.Animation.DROP,
        title: 'Numero: ' + this.Descricao,
        icon: 'Images/notas.png'
    });
    this.marker.setMap(map);
    var IdAnotacao = p.IdAnotacao;
    google.maps.event.addListener(this.marker, 'click', function (event) {
        $("#descricaoAnotacao").val(p.Descricao);
        $('#hiden_idAnotocao').val(IdAnotacao); GlobalMaps.get().MontarModalAnotacao();
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

        //'#lb_IdPoste').text(CodigoGeo); $('#hiden_idAnotocao').val(IdDemanda); $('#hiden_codGeo').val(CodigoGeo); GlobalMaps.get().MontarModalPontoOpcoes();
    });

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
        animation: google.maps.Animation.DROP,
        title: 'Numero: ' + this.Numero,
        icon: 'Images/' + this.NomeImagem + '.png'

    });
    this.marker.setMap(map);
    var CodigoGeo = p.CodGeo;
    google.maps.event.addListener(this.marker, 'click', function (event) {

        if (this.getAnimation() !== null) {
            this.setAnimation(null);
        } else {
            this.setAnimation(google.maps.Animation.BOUNCE);
        }

        $('#lb_IdPoste').text(CodigoGeo); $('#hiden_idDemanda').val(IdDemanda); $('#hiden_codGeo').val(CodigoGeo); GlobalMaps.get().MontarModalPontoOpcoes();


    });

    this.Remove = function () { this.marker.setMap(null) }

}


function PosteMarker(p, map) {
    this.IdPoste = p.IdPoste;
    this.Latitude = p.Latitude;
    this.Longitude = p.Longitude;
    this.NomeImagem = p.Img;
    this.CodGeo = p.CodGeo;
    this.NumeroPosteNaOS = p.NumeroPosteNaOS;
    var NumeroPoste = p.IdPoste;

    if (p.Finalizado == 1) {
        this.marker = new google.maps.Marker({
            position: new google.maps.LatLng(this.Latitude, this.Longitude),
            map: map,
            animation: google.maps.Animation.DROP,
            //title: 'Código do Poste ' + this.CodGeo + ' / Número GPS ' + this.NumeroPosteNaOS,
            title: 'Codigo Poste: ' + this.IdPoste,
            label: {
                color: 'red',
                fontWeight: 'bold',
                text: '' + p.IdPoste
            },
            icon: {
                labelOrigin: new google.maps.Point(11, -5),
                url: 'Images/03.png'
            },
            draggable: true
        });
    } else {
        this.marker = new google.maps.Marker({
            position: new google.maps.LatLng(this.Latitude, this.Longitude),
            map: map,
            animation: google.maps.Animation.DROP,
            //title: 'Código do Poste ' + this.CodGeo + ' / Número GPS ' + this.NumeroPosteNaOS,
            title: 'Codigo Poste: ' + this.IdPoste,
            label: {
                color: 'red',
                fontWeight: 'bold',
                text: '' + p.IdPoste
            },
            icon: {
                labelOrigin: new google.maps.Point(11, -5),
                url: 'Images/' + this.NomeImagem + '.png'
            },
            draggable: true
        });
    }

    this.marker.setMap(map);
    var CodigoGeo = p.CodGeo;

    google.maps.event.addListener(this.marker, 'dragend', function (event) {
        latPoste = this.getPosition().lat();
        lonPoste = this.getPosition().lng();
        idPosteClickado1 = p.IdPoste;

        markerArrastado = this;
        latPosteAntigo = p.Latitude;
        lonPosteAntigo = p.Longitude;


        $("#ModalOpcoesArrastarposte").modal('show');
        return;
    });

    google.maps.event.addListener(this.marker, 'click', function (event) {

        if (this.getAnimation() !== null) {
            this.setAnimation(null);
        } else {
            this.setAnimation(google.maps.Animation.BOUNCE);
        }

        if (criarDemanda) {
            latPoste = p.Latitude;
            lonPoste = p.Longitude;
            idPosteClickado = p.IdPoste;
            //this.marker.setIcon('');
            //this.marker.setAnimation(google.maps.Animation.BOUNCE);
        } else if (criarStrand) {
            if (idPosteClickado == 0) {
                idPosteClickado = p.IdPoste;
            } else {
                idPosteClickado1 = p.IdPoste;
                GlobalMaps.get().CriarNovoStrand();
            }
        }
        else {
            $('#lb_IdPoste').text(CodigoGeo);
            $('#hiden_idPoste').val(NumeroPoste);
            $('#hiden_codGeo').val(CodigoGeo);
            GlobalMaps.get().MontarModalOpcoes();
        }

    });
    this.Remove = function () { this.marker.setMap(null) }
}


function OSMapa(osView, map) {
    this.NumOS = osView.NumeroOrdemServico;
    var NumeroOs = osView.NumeroOrdemServico;
    this.Bound = new google.maps.LatLngBounds();
    this.Colaborador = osView.Colaborador;
    var path = [];
    for (var j = 0; j < osView.PontosPoligono.length; j++) {
        var pl = new google.maps.LatLng(osView.PontosPoligono[j].Lat, osView.PontosPoligono[j].Lon);
        path.push(pl); this.Bound.extend(pl);
    }
    this.PoligonoMapa = new google.maps.Polygon({
        paths: path,
        strokeOpacity: 0.9,
        strokeWeight: 3,
        fillOpacity: 0.25,
        editable: false
    });

    this.PoligonoMapa.infoWindow = new google.maps.InfoWindow({ content: '' });

    this.SetInfoWindowPoligono = function (num_os, situacao, colaborador) {
        switch (situacao) {
            case 0:
                this.PoligonoMapa.infoWindow.setContent('<b>COLABORADOR:</b> ' + colaborador + ' <br /> <b>NÚMERO OS:</b> ' + num_os + ' <br /> <b>SITUAÇÃO OS:</b> ANALISE COMPLETEZA');
                break;
            case 1:
                this.PoligonoMapa.infoWindow.setContent('<b>COLABORADOR:</b> ' + colaborador + ' <br /> <b>NÚMERO OS:</b> ' + num_os + ' <br /> <b>SITUAÇÃO OS:</b> ÁREA DE RISCO');
                break;
            case 2:
                this.PoligonoMapa.infoWindow.setContent('<b>COLABORADOR:</b> ' + colaborador + ' <br /> <b>NÚMERO OS:</b> ' + num_os + ' <br /> <b>SITUAÇÃO OS:</b> BAIXADO');
                break;
            case 3:
                this.PoligonoMapa.infoWindow.setContent('<b>COLABORADOR:</b> ' + colaborador + ' <br /> <b>NÚMERO OS:</b> ' + num_os + ' <br /> <b>SITUAÇÃO OS:</b> BAIXADO EDIÇÃO');
                break;
            case 4:
                this.PoligonoMapa.infoWindow.setContent('<b>COLABORADOR:</b> ' + colaborador + ' <br /> <b>NÚMERO OS:</b> ' + num_os + ' <br /> <b>SITUAÇÃO OS:</b> CONTROLE QUALIDADE');
                break;
            case 5:
                this.PoligonoMapa.infoWindow.setContent('<b>COLABORADOR:</b> ' + colaborador + ' <br /> <b>NÚMERO OS:</b> ' + num_os + ' <br /> <b>SITUAÇÃO OS:</b> DISPONIVEL');
                break;
            case 6:
                this.PoligonoMapa.infoWindow.setContent('<b>COLABORADOR:</b> ' + colaborador + ' <br /> <b>NÚMERO OS:</b> ' + num_os + ' <br /> <b>SITUAÇÃO OS:</b> EM CAMPO');
                break;
            case 7:
                this.PoligonoMapa.infoWindow.setContent('<b>COLABORADOR:</b> ' + colaborador + ' <br /> <b>NÚMERO OS:</b> ' + num_os + ' <br /> <b>SITUAÇÃO OS:</b> FINALIZADO CAMPO');
                break;
            case 8:
                this.PoligonoMapa.infoWindow.setContent('<b>COLABORADOR:</b> ' + colaborador + ' <br /> <b>NÚMERO OS:</b> ' + num_os + ' <br /> <b>SITUAÇÃO OS:</b> FINALIZADO EDIÇÃO');
                break;
            case 9:
                this.PoligonoMapa.infoWindow.setContent('<b>COLABORADOR:</b> ' + colaborador + ' <br /> <b>NÚMERO OS:</b> ' + num_os + ' <br /> <b>SITUAÇÃO OS:</b> FINALIZADO PREPARAÇÃO');
                break;
            case 10:
                this.PoligonoMapa.infoWindow.setContent('<b>COLABORADOR:</b> ' + colaborador + ' <br /> <b>NÚMERO OS:</b> ' + num_os + ' <br /> <b>SITUAÇÃO OS:</b> PREPARAÇÃO');
                break;
            default:
                this.PoligonoMapa.infoWindow.setContent('<b>COLABORADOR:</b> ' + colaborador + ' <br /> <b>NÚMERO OS:</b> ' + num_os + ' <br /> <b>SITUAÇÃO OS:</b> INDISPONIVEL');
        };
    }

    this.SetInfoWindowPoligono(NumeroOs, osView.Situacao, osView.Colaborador);

    google.maps.event.addListener(this.PoligonoMapa, "mousemove", function (e) {
        this.setOptions({ fillOpacity: 0.5 });
        this.infoWindow.setPosition(new google.maps.LatLng({ lat: e.latLng.lat() + 0.00025, lng: e.latLng.lng() }));
        this.infoWindow.open(map);
    });

    google.maps.event.addListener(this.PoligonoMapa, "mouseout", function (e) { this.setOptions({ fillOpacity: 0.35 }); this.infoWindow.close(); });

    this.PoligonoMapa.setMap(map);

    google.maps.event.addListener(this.PoligonoMapa, 'click', function (event) {
        $('.editOS').val('');
        $.ajax({
            type: "GET", url: "/AjaxOrdemDeServico/GetOSByNumero", cache: false,
            headers: { '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
            contentType: 'application/json; charset=utf-8', data: { NumOs: NumeroOs },
            success: function (d) {
                $("#idOSrr").val(NumeroOs);
                $('#numOS').text(NumeroOs);
                $('#status_os').val(d.Situacao);
                $('#dhCricao').val(d.DhCriacao);
                $('#dhEncerramento').val(d.DhEncerramento);
                $('select[name="selectUsers"]').val('' + d.IdUsuario);
                $('#dhQtdPoste').val(d.NumPoste);
                $('#dhQtdPosteFinalizado').val(d.NumPosteFinalizado);
                $('#observacao').val(d.Observacao);
                GlobalMaps.get().MontaBarradeProgresso(Math.round((d.NumPosteFinalizado * 100) / d.NumPoste));
                $('#EditarOS').modal('show');

            },
            error: function (jqXHR, textStatus, errorThrown) { alert(textStatus); }
        });
    });


    this.Remove = function () { this.PoligonoMapa.setMap(null); };

    this.SetColorPoligono = function (situacao) {

        switch (situacao) {
            case 0:
                //ANALISE_COMPLETEZA
                //LARANJA
                this.PoligonoMapa.setOptions({ fillColor: '#ff9900', strokeColor: '#ff9900' });
                break;
            case 1:
                //AREA_DE_RISCO
                //VERDE MUSGO
                this.PoligonoMapa.setOptions({ fillColor: '#339966', strokeColor: '#339966' });
                break;
            case 2:
                //BAIXADO
                //VERDE AGUA
                this.PoligonoMapa.setOptions({ fillColor: '#00ffff', strokeColor: '#00ffff' });
                break;
            case 3:
                //BAIXADO_EDIÇÃO
                //ROSA
                this.PoligonoMapa.setOptions({ fillColor: '#cc0066', strokeColor: '#cc0066' });
                break;
            case 4:
                //CONTROLE_QUALIDADE
                //ROXO
                this.PoligonoMapa.setOptions({ fillColor: '#9900cc', strokeColor: '#9900cc' });
                break;
            case 5:
                //DISPONIVEL
                //VERMELHO
                this.PoligonoMapa.setOptions({ fillColor: '#ff0000', strokeColor: '#ff0000' });
                break;
            case 6:
                //EM CAMPO
                //AZUL
                this.PoligonoMapa.setOptions({ fillColor: '#0000ff', strokeColor: '#0000ff' });
                break;
            case 7:
                //FINALIZADO_CAMPO
                //VERDE
                this.PoligonoMapa.setOptions({ fillColor: '#00ff00', strokeColor: '#00ff00' });
                break;
            case 8:
                //FINALIZADO_EDIÇÃO
                //VERDE ESCURO
                this.PoligonoMapa.setOptions({ fillColor: '#003300', strokeColor: '#003300' });
                break;
            case 9:
                //FINALIZADO_PREPARAÇÃO
                //MARROM
                this.PoligonoMapa.setOptions({ fillColor: '#996633', strokeColor: '#996633' });
                break;
            case 10:
                //PREPARAÇÃO
                //AMARELO
                this.PoligonoMapa.setOptions({ fillColor: '#ffff00', strokeColor: '#ffff00' });
                break;
            default:
                //PRETO
                this.PoligonoMapa.setOptions({ fillColor: '#000000', strokeColor: '#000000' });
        }
    };

    this.SetColorPoligono(osView.Situacao);


}

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

var GlobalMaps = (function () {
    var instance, map, panorama, OS = [], MapsLabels = [], LimiteCidade, CidadeSelected, drawingManager, latlongs = [], shapeOs, shapeSelecionadoCrregado;
    var RedePostes = [], RedeDemandas = [], RedeStrands = [], RedeVaosDemanda = [], RedeAnotacao = [];
    var PlotaeDesplota = false;
    var PlotaeDesplotaDemanda = false;
    var PlotaeDesplotaStrands = false;
    var PlotaeDesplotaAnotacao = false;
    var identificadores_foto_ponto = [];
    var NovaAnotacao;
    var InicioStrand;
    var Localusers = new Map();
    var cont_foto_ponto = 0;
    var novaCidade = true;
    var geocoder;
    var boundsRect;

    var PostesId = [];

    var criarPoste = false;
    var criarAnotacao = false;

    var latStrand1 = 0, lonStrand1 = 0, latStrand2 = 0, lonStrand2 = 0;

    $('[data-toggle="novacidadetooltip"]').tooltip();

    function ClearPostes() {
        for (var i = 0; i < RedePostes.length; i++) {
            RedePostes[i].Remove();
        }
        RedePostes = [];
        PlotaeDesplota = false;
        $("#plotaElimpaPoste").text(" Exibir Postes");
    }

    function ClearDemandas() {
        for (var i = 0; i < RedeDemandas.length; i++) {
            RedeDemandas[i].Remove();
        }
        RedeDemandas = [];
        PlotaeDesplotaDemanda = false;
        $("#PlotaeDesplotaDemanda").text(" Exibir Demandas");
    }

    function ClearTitulosPOlygonos() {
        for (var i = 0; i < MapsLabels.length; i++) {
            MapsLabels[i].Remove();
        }
        MapsLabels = [];
        // PlotaeDesplotaDemanda = false;
        //$("#PlotaeDesplotaDemanda").text(" Exibir Demandas");
    }

    function ClearRedeVaosDemanda() {
        for (var i = 0; i < RedeVaosDemanda.length; i++) {
            RedeVaosDemanda[i].Remove();
        }
        RedeVaosDemanda = [];
    }

    function ClearStrands() {
        for (var i = 0; i < RedeStrands.length; i++) {
            RedeStrands[i].Remove();
        }
        RedeStrands = [];
        PlotaeDesplotaStrands = false;
        $("#PlotaeDesplotaStrands").text(" Exibir Strands");
    }

    function ClearAnotacoes() {
        for (var i = 0; i < RedeAnotacao.length; i++) {
            RedeAnotacao[i].Remove();
        }
        RedeAnotacao = [];
        PlotaeDesplotaAnotacao = false;
        $("#PlotaeDesplotaAnotacao").text(" Exibir Anotações");
    }

    function ClearMapLabels() {
        for (var i = 0; i < MapsLabels.length; i++) {
            MapsLabels[i].Remove();
        }
        MapsLabels = [];       
    }


    function LoadMap() {
        map = new google.maps.Map($('#globalMap').get(0), {
            center: new google.maps.LatLng(-22.906631919877885, -47.059739862060496),
            zoom: 10,
            zoomControl: true,
            mapTypeControl: true,
            mapTypeControlOptions: {
                style: google.maps.MapTypeControlStyle.LEFT_BOTTOM,
                position: google.maps.ControlPosition.LEFT_BOTTOM
            }
        });

        geocoder = new google.maps.Geocoder();
        // Create overlays for geocoding result representation..
        boundsRect = new google.maps.Rectangle();
        boundsRect.setOptions({
            clickable: false,
            fillOpacity: 0,
            strokeColor: '#FF0000',
            strokeOpacity: 1,
            strokeWeight: 1
        });

        initAutocompleteRua();
        initAutocomplete();
        InicioLocaluser();

        drawingManager = new google.maps.drawing.DrawingManager({
            drawingMode: null,

            drawingControl: true,
            drawingControlOptions: {
                position: google.maps.ControlPosition.LEFT_CENTER,
                drawingModes: ['marker', 'circle', 'polygon', 'polyline', 'rectangle']
            },
            polygonOptions: {
                fillColor: '#0000ff',
                strokeColor: '#0000ff',
                clickable: true,
                editable: false,
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillOpacity: 0.35,
                zIndex: 1
            }
        });
        drawingManager.setMap(map);

        drawingManager.setOptions({
            drawingControl: false
        });

        google.maps.event.addListener(drawingManager, 'polygoncomplete', function (polygon) {
            //latlons = polygon.getPath();
            polygon.getPath().forEach(function (latLng) { latlongs.push(latLng.toString()); })
            $('#CriarOS').modal('show');
            shapeOs = polygon;
        });

        PreencheHtmlDOMCidadesHome();

        map.addListener('click', function (event) {
            if (criarPoste) {
                var NovoMaker = new google.maps.Marker({
                    position: event.latLng,
                    map: map,
                    title: 'Novo Poste'
                });

                SalvarNovoPoste(NovoMaker);
                DesSelecionarPoste();
            }
            if (criarAnotacao) {
                NovaAnotacao = new google.maps.Marker({
                    position: event.latLng,
                    map: map,
                    title: 'Novo Anotacao'
                });

                $("#ModalDescricaoAnotacao").modal('show');
                $("#descricaoAnotacao").val("");
            }

            if (criarDemanda) {
                NovaDemandaPin = new google.maps.Marker({
                    position: event.latLng,
                    map: map,
                    title: 'Novo Demanda'
                });

                $("#ModalNovoPontoEntrega").modal('show');

            }
            if (criarStrand) {

                if (latStrand1 == 0) {
                    latStrand1 = event.latLng.lat();
                    lonStrand1 = event.latLng.lng();

                    InicioStrand = new google.maps.Marker({
                        position: event.latLng,
                        map: map
                    });

                } else {
                    latStrand2 = event.latLng.lat();
                    lonStrand2 = event.latLng.lng();
                    InicioStrand.setMap(null);
                    CriarNovoStrandNoMapa();

                }
            }
        });
    }
    function NovoPoste() {
        if (criarPoste) {
            DesSelecionarPoste();
        } else {
            SelecionarPoste();
            DesSelecionarAnotacao();
            DesSelecionaDemanda();
            DesSelecionaStrand();
        }
    }

    function DesSelecionarPoste() {
        criarPoste = false;
        $("#btn_novo_poste").removeClass('btn btn-warning');
        $("#btn_novo_poste").addClass('btn btn-info');
    }
    function SelecionarPoste() {
        criarPoste = true;
        $("#btn_novo_poste").removeClass('btn btn-info');
        $("#btn_novo_poste").addClass('btn btn-warning');
    }

    function NovoAnotacao() {
        if (criarAnotacao) {
            DesSelecionarAnotacao();
        } else {
            SelecionarAnotacao();
            DesSelecionaDemanda();
            DesSelecionarPoste();
            DesSelecionaStrand();
        }
    }

    function DesSelecionarAnotacao() {
        criarAnotacao = false;
        $("#btn_novo_anotacao").removeClass('btn btn-warning');
        $("#btn_novo_anotacao").addClass('btn btn-info');
    }
    function SelecionarAnotacao() {
        criarAnotacao = true;
        $("#btn_novo_anotacao").removeClass('btn btn-info');
        $("#btn_novo_anotacao").addClass('btn btn-warning');
    }

    function NovaDemanda() {
        if (criarDemanda) {
            DesSelecionaDemanda();
        } else {
            SelecionaDemanda();
            DesSelecionarPoste();
            DesSelecionarAnotacao();
            DesSelecionaStrand();
        }
    }

    function DesSelecionaDemanda() {
        $("#btn_nova_demanda").removeClass('btn btn-warning');
        $("#btn_nova_demanda").addClass('btn btn-info');
        latPoste = 0;
        lonPoste = 0;
        idPosteClickado = 0;
        idPosteClickado1 = 0;
        criarDemanda = false;
    }
    function SelecionaDemanda() {
        $("#btn_nova_demanda").removeClass('btn btn-info');
        $("#btn_nova_demanda").addClass('btn btn-warning');
        criarDemanda = true;
    }


    function NovoStrand() {
        if (criarStrand) {
            DesSelecionaStrand();
        } else {
            SelecionaStrand();
            DesSelecionarPoste();
            DesSelecionarAnotacao();
            DesSelecionaDemanda();

        }
    }

    function SelecionaStrand() {
        $("#btn_novo_strand").removeClass('btn btn-info');
        $("#btn_novo_strand").addClass('btn btn-warning');
        criarStrand = true;
    }

    function DesSelecionaStrand() {
        $("#btn_novo_strand").removeClass('btn btn-warning');
        $("#btn_novo_strand").addClass('btn btn-info');
        criarStrand = false;
        if (InicioStrand != null) {
            InicioStrand.setMap(null);
        }
        latStrand1 = 0;
    }

    function CancelaAnotacao() {
        NovaAnotacao.setMap(null);
        HideAllModal();
        DesSelecionarAnotacao();
    }


    function CancelaNovaDemanda() {
        NovaDemandaPin.setMap(null);
        HideAllModal();
        DesSelecionaDemanda();
    }

    function verPosteStreet() {

        $("#globalMap").css("width", "50%");
        $("#pano").css("display", "block");

        HideAllModal();

        var idPosteclicado = $('#hiden_idPoste').attr("value");
        var pano = $("#pano2");
        var fenway;

        CallServer("GET", "/AjaxRede/GetPosteById", { IdPoste: idPosteclicado },
        function (retorno) {
            fenway = { lat: retorno.Latitude, lng: retorno.Longitude };

            var pano = $("#pano2");
            $("#pano").css("float", "right");
            $("#pano").css("height", "650px");
            $("#pano").css("width", "50%");


            var panorama = new google.maps.StreetViewPanorama($("#pano").get(0), {
                position: fenway,
                pov: {
                    heading: 34,
                    pitch: 10
                }
            });

            map.setStreetView(panorama);
            $("#btn_desativarLocalStreet").prop("disabled", false);
        });

        //   var fenway = { lat: -22.906631919877885, lng: -47.059739862060496 };        
    }

    function DesativarLocalStreet() {
        $("#globalMap").css("width", "100%");
        $("#pano").css("display", "none");
        map.setStreetView(null);
        $("#btn_desativarLocalStreet").prop("disabled", true);
    }

    function pegandoPolygon(shape) {
        shapeSelecionadoCrregado = shape;
    }



    function ApagarOrdem() {

        var r = confirm("Deseja Apagar Ordem de Serviço?");
        if (r == true) {
            var numeroOrdem = $("#idOSrr").val();
            $("#carregaOrdem").css("display", "block"); 
            CallServer("POST", "/AjaxOrdemDeServico/ApagarOrdemAndPoligonos", { numeroOs: numeroOrdem },
                function (r) {
                    if (r.Msg == "OK") {
                        $('#CriarOS').modal('hide');

                        for (var i = 0; i < OS.length; i++) {
                            if (OS[i].NumOS == r.NumeroOs) {
                                OS[i].Remove();
                                $("#carregaOrdem").css("display", "none");
                                // OS[i].SetColorPoligono(r.OS_Return.Situacao);
                                //  OS[i].SetInfoWindowPoligono(r.OS_Return.NumeroOrdemServico, r.OS_Return.Situacao, r.OS_Return.Colaborador);
                            }
                        }
                    }
                });
        } else {
        }


    }

    function ConcelarCricaoOs(polygon) {
        shapeOs.setMap(null);
    }


    function CallServer(_type, _url, _param, _method) {
        Pace.start();
        $.ajax({
            type: _type, url: _url, cache: false,
            headers: { '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
            dataType: 'json', data: _param,
            success: function (d) { _method(d); },
            error: function (jqXHR, textStatus, errorThrown) {
                alert($.parseJSON(jqXHR.responseText).Msg);
            }
        }).always(function () { Pace.stop(); });
    }

    function PreencheHtmlDOMCidades() {
        CallServer("GET", "/AjaxOrdemDeServico/GetCidades", {}, function (d) {
            var $select = $('#selectCidade');
            $select.find('option').remove();
            $.each(d, function (key, value) {
                $select.append('<option value=' + value.IdCidade + '>' + value.Nome + '</option>');
            });
            $('#ModalCidade').modal('show');
        });
    }

    function PreencheHtmlDOMCidadesHome() {        
        $('#selectcityHome').find('option:not(:first)').remove();
        CallServer("GET", "/AjaxOrdemDeServico/GetCidades", {}, function (d) {
            var $select = $('#selectcityHome');

            //  $select.find('option').remove();
            $.each(d, function (key, value) {
                $select.append('<option value=' + value.IdCidade + '>' + value.Nome + '</option>');
            });
        });
    }

    function CarregaOrdemPorCidade() {
        //$('#selectOrdemHome').find('option:not(:first)').text("TODAS");

        drawingManager.setOptions({
            drawingControl: true
        });
        CidadeSelected = $("#selectcityHome option:selected").val();
        var pode = parseInt($("#selectcityHome :selected").val());
        $('#selectOrdemHome').find('option:not(:first)').remove();
        if (pode != 0) {
            $("#carregaOrdem").css("display", "block");
            idCidade = $("#selectcityHome :selected").val();
            CallServer("GET", "/AjaxOrdemDeServico/GetOSByCidadeHome", { idCidade: idCidade }, function (Result) {
                if (Result.ListaOs != null) {
                    $("#carregaOrdem").css("display", "none");
                    $select = $("#selectOrdemHome");
                    $.each(Result.ListaOs, function (key, data) {
                        $select.append('<option value=' + data.NumeroOrdemServico + '>' + data.NumeroOrdemServico + '</option>');
                    });
                }
                //  $('#selectOrdemHome option:contains("----------")').text('TODAS');
            });
        }
        codeAddress();
        $("#campoSearchRua").css("display", "block");
    }

    function PlotOsByCidade() {

        if ($("#selectOrdemHome :selected").val() == 0) {
            $("#infoOs").css("display", "none");

            PlotaeDesplota = false;
            PlotaeDesplotaDemanda = false;
            PlotaeDesplotaStrands = false;
            $("#plotaElimpaPoste").text(" Exibir Postes");
            $("#PlotaeDesplotaDemanda").text(" Exibir Demandas");
            $("#PlotaeDesplotaStrands").text(" Exibir Strands");

            ClearDemandas();
            ClearPostes();
            ClearRedeVaosDemanda();
            ClearStrands();
            ClearAnotacoes();
            ClearTitulosPOlygonos();

            CidadeSelected = $("#selectcityHome option:selected").val();
            $("#caixaMenuCriacao").css("display", "none");
            $("#menuExibirEquipamentos").css("display", "block");            
            CarregaCidade(CidadeSelected);            

            DesativarLocalStreet();
        } else {
            ClearMapLabels();
            $("#caixaMenuCriacao").css("display", "block");
            $("#menuExibirEquipamentos").css("display", "none");
            PlotaPosteByOs($("#selectOrdemHome :selected").val());
        }
    }

    function Refresh() {
        if (CidadeSelected === undefined) {
            bootbox.alert("Nenhuma Cidade Carregada !");
        }
        else {
            CarregaCidade(CidadeSelected);
            //PlotaPostesByCidade(CidadeSelected);
        }
    }



    function CarregaCidade(Id_Cidade) {

        var lowx,
       highx,
       lowy,
       highy,
       lats = [],
       lngs = [],
       vertices;

        $("#carregaOrdem").css("display", "block");
        CallServer("GET", "/AjaxOrdemDeServico/GetOSByCidade", { idCidade: Id_Cidade }, function (retorno) {
            if (retorno.ListaOs.length == 0) {
                alert("Não existe O.S. para esta Cidade.");
                $("#carregaOrdem").css("display", "none");
            }
            else {
                for (var i = 0; i < OS.length; i++) { OS[i].Remove(); }
                OS = [];
                for (var i = 0; i < retorno.ListaOs.length; i++)
                {
                    OS.push(new OSMapa(retorno.ListaOs[i], map));
                }

                $.each(OS, function (i, item) {
                    MapsLabels.push(new MapLabelsHome(item, map)); 
                });

                map.setCenter(OS[0].Bound.getCenter());
                map.setZoom(15);
                $('#ModalCidade').modal('hide');
                $("#carregaOrdem").css("display", "none");
                //hAbitlita Controle para desenhar shapes

            }
            if (LimiteCidade != null) {
                LimiteCidade.Remove();
            }
            if (retorno.Limites != null) {
                LimiteCidade = null;
                LimiteCidade = new Limetes(retorno.Limites, map);
            }
        });
    }


    //plota os postes de uma cidade
    function PlotaPostesByCidade() {

        if (!PlotaeDesplota) {
            $("#carregaOrdem").css("display", "block");
            CidadeSelected = $("#selectcityHome option:selected").val();

            //Id_Cidade = $("#selectCidade option:selected").val();
            if (CidadeSelected === undefined || CidadeSelected == "") return;
            // ClearArvores();
            //ClearVaosDemandas();
            // PlotaArvoresByCidade(Id_Cidade);
            // PlotaQuadrasByCidade(Id_Cidade);
            CallServer("GET", "/AjaxRede/GetPostesByCidade", { idCidade: CidadeSelected },
                function (Result) {

                    if (Result.Postes != null) {
                        window.history.pushState("", "Visium Geo - Index", window.location.href.split('?')[0]); // Alterando a URL HTML5
                        //  ClearPostes();
                        TipoFiltro = "";
                        // LimpaLegenda();
                        CidadeSelected = Result.Postes.IdCidade;
                        $("#carregaOrdem").css("display", "none");
                        CreatePostes(Result.Postes);

                        $("#plotaElimpaPoste").text(" Esconder Postes");
                        PlotaeDesplota = true;
                    } else {
                        $("#carregaOrdem").css("display", "none");
                        alert('Não há postes nessa cidade');
                    }

                    //if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
                    //else {
                    //    window.history.pushState("", "Visium Geo - Index", window.location.href.split('?')[0]); // Alterando a URL HTML5
                    //    //  ClearPostes();
                    //    TipoFiltro = "";
                    //    // LimpaLegenda();
                    //    CidadeSelected = Result.Postes.IdCidade;
                    //  //  $("#labelCidade").text("Cidade: " + Result.Postes.NomeCidade);
                    //  //  $("#labelOs").hide();
                    //    CreatePostes(Result.Postes);

                    //    /*if (LimiteCidade != null) {
                    //        LimiteCidade.Remove();
                    //    }
                    //    if (Result.Limites != null) {
                    //        LimiteCidade = null;
                    //        LimiteCidade = new Limetes(Result.Limites, map);
                    //    }*/
                    //}
                }
            );
            

            

        } else {
            ClearPostes();
        }
    }

    //Add postes no map e no array de postes do java script
    function CreatePostes(Result) {
        //if (Result.Postes.length == 0) { bootbox.alert("Não existe Postes para esta consulta."); return; }
        RedePostes = [];
        var quantNA = 0;
        var quantSA = 0;
        for (var i = 0; i < Result.Postes.length; i++) {
            //caso um filtro esteja sendo aplicado os postes sao submetidos a esses metodos.
            if (TipoFiltro != undefined && TipoFiltro != "") {
                if (TipoFiltro == "Potencia") { Result.Postes[i].Img = AtribuiImgFiltroPotencia(Result.Postes[i].Img); }
                else if (TipoFiltro == "Lampada") { Result.Postes[i].Img = AtribuiImgFiltroLampada(Result.Postes[i].Img); }
                else if (TipoFiltro == "Status") { Result.Postes[i].Img = Result.Postes[i].Img; }
                else if (TipoFiltro == "NaSa") {
                    Result.Postes[i].Img = Result.Postes[i].Img;
                    if (Result.Postes[i].Img == "17") { quantNA++; }
                    else if (Result.Postes[i].Img == "19") { quantSA++; }
                }
            }
            RedePostes.push(new PosteMarkerHome(Result.Postes[i], map));
        }
        /*if (TipoFiltro == "NaSa") {
            MontaLegendaNaSa(quantNA, quantSA);
        }*/

        map.setZoom(14);
        //    if (Result.Centro.Lat && Result.Centro.Lon) {
        map.setCenter(new google.maps.LatLng(Result.Centro.Lat, Result.Centro.Lon));
        //}
        //HideAllModal();
    }

    //plota os Demandas de uma cidade
    function PlotaDemandasByCidade() {

        if (!PlotaeDesplotaDemanda) {

            $("#carregaOrdem").css("display", "block");
            CidadeSelected = $("#selectcityHome option:selected").val();
            PlotaVaosDemandas(CidadeSelected);

            //Id_Cidade = $("#selectCidade option:selected").val();
            if (CidadeSelected === undefined || CidadeSelected == "") return;
            // ClearArvores();
            //ClearVaosDemandas();
            // PlotaArvoresByCidade(Id_Cidade);
            // PlotaQuadrasByCidade(Id_Cidade);
            CallServer("GET", "/AjaxRede/GetDemandasByCidade", { idCidade: CidadeSelected },
                function (Result) {

                    if (Result.PontoEntregas != null) {
                        window.history.pushState("", "Visium Geo - Index", window.location.href.split('?')[0]); // Alterando a URL HTML5
                        //  ClearPostes();
                        TipoFiltro = "";
                        // LimpaLegenda();
                        CidadeSelected = Result.PontoEntregas.IdCidade;
                        //  $("#labelCidade").text("Cidade: " + Result.PontoEntregas.NomeCidade);
                        //$("#labelOs").hide();
                        $("#carregaOrdem").css("display", "none");
                        CreateDemandas(Result.PontoEntregas);
                        $("#PlotaeDesplotaDemanda").text(" Esconder Demandas");
                        PlotaeDesplotaDemanda = true;
                    } else {
                        alert('Não há demandas nessa cidade');
                        $("#carregaOrdem").css("display", "none");
                    }

                    //if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
                    //else {
                    //    window.history.pushState("", "Visium Geo - Index", window.location.href.split('?')[0]); // Alterando a URL HTML5
                    //    //  ClearPostes();
                    //    TipoFiltro = "";
                    //    // LimpaLegenda();
                    //    CidadeSelected = Result.PontoEntregas.IdCidade;
                    //  //  $("#labelCidade").text("Cidade: " + Result.PontoEntregas.NomeCidade);
                    //    //$("#labelOs").hide();
                    //    CreateDemandas(Result.PontoEntregas);

                    //    /*if (LimiteCidade != null) {
                    //        LimiteCidade.Remove();
                    //    }
                    //    if (Result.Limites != null) {
                    //        LimiteCidade = null;
                    //        LimiteCidade = new Limetes(Result.Limites, map);
                    //    }*/
                    //}
                }
            );

           
        } else {
            ClearDemandas();
            ClearRedeVaosDemanda();

        }
    }

    //Add postes no map e no array de postes do java script
    function CreateDemandas(Result) {
        //if (Result.Postes.length == 0) { bootbox.alert("Não existe Postes para esta consulta."); return; }
        RedeDemandas = [];
        var quantNA = 0;
        var quantSA = 0;
        for (var i = 0; i < Result.PontoEntregas.length; i++) {
            //caso um filtro esteja sendo aplicado os postes sao submetidos a esses metodos.
            if (TipoFiltro != undefined && TipoFiltro != "") {
                if (TipoFiltro == "Potencia") { Result.PontoEntregas[i].Img = AtribuiImgFiltroPotencia(Result.PontoEntregas[i].Img); }
                else if (TipoFiltro == "Lampada") { Result.PontoEntregas[i].Img = AtribuiImgFiltroLampada(Result.PontoEntregas[i].Img); }
                else if (TipoFiltro == "Status") { Result.PontoEntregas[i].Img = Result.PontoEntregas[i].Img; }
                else if (TipoFiltro == "NaSa") {
                    Result.PontoEntregas[i].Img = Result.PontoEntregas[i].Img;
                    if (Result.PontoEntregas[i].Img == "17") { quantNA++; }
                    else if (Result.PontoEntregas[i].Img == "19") { quantSA++; }
                }
            }
            RedeDemandas.push(new DemandaMarkerHome(Result.PontoEntregas[i], map));
        }
        /*if (TipoFiltro == "NaSa") {
            MontaLegendaNaSa(quantNA, quantSA);
        }*/

        map.setZoom(14);
        //if (Result.Centro.Lat && Result.Centro.Lon) {
        map.setCenter(new google.maps.LatLng(Result.Centro.Lat, Result.Centro.Lon));
        //}
        //HideAllModal();


    }


    //plota os postes de uma cidade
    function PlotaStrandsByCidade() {

        if (!PlotaeDesplotaStrands) {

            $("#carregaOrdem").css("display", "block");
            CidadeSelected = $("#selectcityHome option:selected").val();

            //Id_Cidade = $("#selectCidade option:selected").val();
            if (CidadeSelected === undefined || CidadeSelected == "") return;
            // ClearArvores();
            //ClearVaosDemandas();
            // PlotaArvoresByCidade(Id_Cidade);
            // PlotaQuadrasByCidade(Id_Cidade);
            CallServer("GET", "/AjaxRede/GetStrands", { idCidade: CidadeSelected },
                function (Result) {

                    if (Result.DemandaStrands != null) {
                        TipoFiltro = "";
                        // LimpaLegenda();
                        CidadeSelected = Result.DemandaStrands.IdCidade;
                        // $("#labelCidade").text("Cidade: " + Result.DemandaStrands.NomeCidade);
                        //  $("#labelOs").hide();
                        $("#carregaOrdem").css("display", "none");
                        CreateStrands(Result.DemandaStrands);
                        $("#PlotaeDesplotaStrands").text("Esconder Strands");
                        PlotaeDesplotaStrands = true;
                    }else{
                        $("#carregaOrdem").css("display", "none");
                        alert('Não há strands nessa cidade');
                    }


                    //if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
                    //else {
                    //    //   window.history.pushState("", "Visium Geo - Index", window.location.href.split('?')[0]); // Alterando a URL HTML5
                    //    //  ClearPostes();
                    //    TipoFiltro = "";
                    //    // LimpaLegenda();
                    //    CidadeSelected = Result.DemandaStrands.IdCidade;
                    //   // $("#labelCidade").text("Cidade: " + Result.DemandaStrands.NomeCidade);
                    //  //  $("#labelOs").hide();
                    //    CreateStrands(Result.DemandaStrands);

                    //    /*if (LimiteCidade != null) {
                    //        LimiteCidade.Remove();
                    //    }
                    //    if (Result.Limites != null) {
                    //        LimiteCidade = null;
                    //        LimiteCidade = new Limetes(Result.Limites, map);
                    //    }*/
                    //}
                }
            );
           
        } else {
            ClearStrands();
        }
    }


    function CreateStrands(Result) {
        //if (Result.Postes.length == 0) { bootbox.alert("Não existe Postes para esta consulta."); return; }
        RedeStrands = [];

        for (var i = 0; i < Result.DemandaStrands.length; i++) {
            //caso um filtro esteja sendo aplicado os postes sao submetidos a esses metodos.          
            RedeStrands.push(new StrandsHome(Result.DemandaStrands[i], map));
        }
        /*if (TipoFiltro == "NaSa") {
            MontaLegendaNaSa(quantNA, quantSA);
        }*/

        map.setZoom(14);
        //   if (Result.Centro.Lat && Result.Centro.Lon) {
        map.setCenter(new google.maps.LatLng(Result.Centro.Lat, Result.Centro.Lon));
        //   }
        //HideAllModal();
    }


    //carrega Postes de uma OS
    function PlotaVaosDemandas(idCidade) {
        CallServer("GET", "/Vao/GetVaosDemandasByCidade", { idCidade: idCidade },
		function (Result) {
		    //  if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
		    // ClearPontoEntrega();
		    TipoFiltro = "";
		    //LimpaLegenda();
		    //Id_Cidade = Result.Postes.IdCidade;
		    /*$("#labelCidade").text("Cidade: " + Result.Postes.NomeCidade);
		    $("#labelOs").show();
		    $("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));*/
		    CreateVaosDemandas(Result.VaosDemandas);
		});
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
            RedeVaosDemanda.push(new VaoDemandasHome(Result.VaosDemandas[i], map));
        }
        /*  if (TipoFiltro == "NaSa") {
              MontaLegendaNaSa(quantNA, quantSA);
          }*/

        map.setZoom(14);
        /*if (Result.Centro.Lat && Result.Centro.Lon) { map.setCenter(new google.maps.LatLng(Result.Centro.Lat, Result.Centro.Lon)); }
        HideAllModal();*/
    }

    //carrega Postes de uma OS
    function PlotaAnotacoesByOs() {
        CidadeSelected = $("#selectcityHome option:selected").val();
        CallServer("GET", "/Anotacoes/GetAnotacaoesByCidade", { idCidade: CidadeSelected },
		function (Result) {
		    // if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
		    // ClearPontoEntrega();
		    TipoFiltro = "";
		    //LimpaLegenda();
		    //Id_Cidade = Result.Postes.IdCidade;
		    /*$("#labelCidade").text("Cidade: " + Result.Postes.NomeCidade);
		    $("#labelOs").show();
		    $("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));*/
		    if (Result.Anotacoes != null) {
		        CreateAnotacao(Result);
		    }
		    /*if (LimiteCidade != null) {
		        LimiteCidade.Remove();
		    }
		    if (Result.Limites != null) {
		        LimiteCidade = null;
		        LimiteCidade = new Limetes(Result.Limites, map);
		    }*/
		});
    }


    //Criar os markers de Anotacoes
    function CreateAnotacao(Result) {
        RedeAnotacao = [];
        //  if (Result.VaosDemandas.length == 0) { bootbox.alert("Não existe Anotações para esta consulta."); return; }
        var quantNA = 0;
        var quantSA = 0;
        for (var i = 0; i < Result.Anotacoes.length; i++) {
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
            RedeAnotacao.push(new AnotacaoMarker(Result.Anotacoes[i], map));
        }
        /*  if (TipoFiltro == "NaSa") {
              MontaLegendaNaSa(quantNA, quantSA);
          }*/

        map.setZoom(14);
        //  if (Result.Centro.Lat && Result.Centro.Lon) {
        map.setCenter(new google.maps.LatLng(Result.Centro.Lat, Result.Centro.Lon));
        //  }
        /*HideAllModal();*/
    }

    function EditarOsView() {
        CallServer("POST", "/AjaxOrdemDeServico/SalvarEdicaoOS",
            {
                NumOs: $("#idOSrr").val(),
                DhFinal: $("#dhEncerramento").val(),
                IdUsuario: $("#selectUsers option:selected").val(),
                Situacao: $("#status_os").val(),
                Observacao: $("#observacao").val(),
            },
            function (r) {
                if (r.Msg == "OK") {
                    $('#EditarOS').modal('hide');
                    for (var i = 0; i < OS.length; i++) {
                        if (OS[i].NumOS == r.OS_Return.NumeroOrdemServico) {
                            OS[i].SetColorPoligono(r.OS_Return.Situacao);
                            OS[i].SetInfoWindowPoligono(r.OS_Return.NumeroOrdemServico, r.OS_Return.Situacao, r.OS_Return.Colaborador);
                        }
                    }
                }
                else { alert(r.ErroMsg); }
            });
    }


    function CriarOsView() {

        var nomeOrdem = $("#nomeOrdemCriada").val();
        if (nomeOrdem != "") {
            var PostesSelecionados = [];
            var StrandsSelecionados = [];
            var todos;
            for (var i = 0; i < RedePostes.length; i++) {
                var resultContem = google.maps.geometry.poly.containsLocation(new google.maps.LatLng(RedePostes[i].Latitude, RedePostes[i].Longitude), shapeOs);
                if (resultContem) {
                    PostesSelecionados.push(RedePostes[i].IdPoste);
                }
            }

            for (var i = 0; i < RedeStrands.length; i++) {
                var resultContemStrands = google.maps.geometry.poly.containsLocation(new google.maps.LatLng(RedeStrands[i].X1, RedeStrands[i].Y1), shapeOs);
                if (resultContemStrands) {
                    StrandsSelecionados.push(RedeStrands[i].IdDemandaStrand);
                }
            }

            CallServer("POST", "/AjaxOrdemDeServico/CriarOsPeloSite", {
                latlons: latlongs,
                PostesSelecionados: PostesSelecionados,
                StrandsSelecionados: StrandsSelecionados,
                IdCidade: CidadeSelected,
                NomeOrdem: $("#nomeOrdemCriada").val(),
                IdUsuario: $("#selectUsers_cria option:selected").val(),
                Situacao: $("#status_os_cria option:selected").val(),
                Observacao: $("#observacao").val(),
            },
                function (r) {
                    if (r.Status == 2) {
                        $('#CriarOS').modal('hide');
                    }
                });

            latlongs = [];
            PostesSelecionados = [];
        } else {
            alert('Digite o nome da ordem sem espaços');
            $("#nomeOrdemCriada").attr('placeholder', 'Digite algo aqui sem espaços');
        }        
    }

    function LoadPostesByOs() {
        //   window.location.href = "/Rede?PosteByOs=" + $("#idOSrr").val();

        $("#infoOs").css("display", "block");
        var numroOS = $("#numOS").text();
        //   $("#selectOrdemHome").removeAttr('selected').filter('[value='+numroOS+']').prop('selected', true);
        //var numroOS = $("#selectOrdemHome  :selected").val();

        $('#selectOrdemHome option').each(function () {
            if ($(this).val() == numroOS) {
                $(this).prop("selected", true);
            }
        });
        ClearMapLabels();
        PlotaPosteByOs(numroOS);
    }



    //Limpa Postes do map e Array de poste
    // function ClearPostes() { for (var i = 0; i < RedePostes.length; i++) { RedePostes[i].Remove(); } RedePostes = []; }
    //function ClearArvores() { for (var i = 0; i < Arvores.length; i++) { Arvores[i].Remove(); } Arvores = []; }
    // function ClearPontoEntrega() { for (var i = 0; i < RedePontoEntrega.length; i++) { RedePontoEntrega[i].Remove(); } RedePontoEntrega = []; }
    //  function ClearVaosDemandas() { for (var i = 0; i < RedeVaosDemanda.length; i++) { RedeVaosDemanda[i].Remove(); } RedeVaosDemanda = []; }

    function PlotaPosteByOs(numroOS) {
        $("#carregaOrdem").css("display", "block");
        ClearDemandas();
        ClearRedeVaosDemanda();
        ClearAnotacoes();
        ClearStrands();

        PlotaVaosDemandasByOs(numroOS);
        PlotaPontoEntregaByOs(numroOS);
        PlotaAnotacoesByOs(numroOS);
        PlotaStrandsByOs(numroOS);
        //  PlotaVaosDemandasByOs($.GetParamUrl('PosteByOs'));
        //  PlotaAnotacoesByOs($.GetParamUrl('PosteByOs'));
        //  PlotaStrandsByOs($.GetParamUrl('PosteByOs'));
        //  PlotaQuadasByOs($.GetParamUrl('PosteByOs'));
        CallServer("GET", "/AjaxRede/GetPostesByOs", { codOs: numroOS },
		function (Result) {
		    //  if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
		    ClearPostes();
		    TipoFiltro = "";
		    LimpaLegenda();
		    Id_Cidade = Result.Informacao.IdCidade;
		    for (var i = 0; i < OS.length; i++) { OS[i].Remove(); }
		    OS = [];

		    $("#caixaMenuCriacao").css("display", "block");
		    $("#menuExibirEquipamentos").css("display", "none");
		    $("#carregaOrdem").css("display", "none");
		    //  $("#labelCidade").text("Cidade: " + Result.Informacao.NomeCidade);
		    //   $("#labelOs").show();
		    //   $("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));
		    if (Result.Postes != null) {
		        CreatePostesOs(Result.Postes);
		    }

		    if (LimiteCidade != null) {
		        LimiteCidade.Remove();
		    }
		    if (Result.Limites != null) {
		        LimiteCidade = null;
		        LimiteCidade = new Limetes(Result.Limites, map);
		    }
		    //if (Result.Postes.Centro.Lat && Result.Centro.Lon) {
		    ////map.setCenter(new google.maps.LatLng(Result.Limites.LatitudeA, Result.Limites.LongitudeB));
		    //    if (Result.Postes.Centro.Lat && Result.Postes.Centro.Lon) {
		    //        map.setCenter(new google.maps.LatLng(Result.Limites.LatitudeA, Result.Limites.LongitudeB));
		    //    }
		    //}

		    drawingManager.setOptions({
		        drawingControl: true
		    });


		    map.setCenter(new google.maps.LatLng(Result.Limites[0].LatitudeA, Result.Limites[0].LongitudeA));
		    map.setZoom(16);
		    HideAllModal()


		});
    }

    //carrega Ponto de Entrega de uma OS
    function PlotaPontoEntregaByOs(ordemServico) {
        ClearRedeVaosDemanda();
        CallServer("GET", "/PontodeEntrega/GetPontoEntregaByOs", { codOs: ordemServico },
		function (Result) {
		    //    if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
		    ClearDemandas();
		    TipoFiltro = "";
		    //LimpaLegenda();
		    //Id_Cidade = Result.Postes.IdCidade;
		    /*$("#labelCidade").text("Cidade: " + Result.Postes.NomeCidade);
		    $("#labelOs").show();
		    $("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));*/
		    if (Result.PontoEntregas != null) {
		        CreatePontoEntrega(Result.PontoEntregas);
		    }
		    //if (LimiteCidade != null) {
		    //    LimiteCidade.Remove();
		    //}
		    //if (Result.Limites != null) {
		    //    LimiteCidade = null;
		    //    LimiteCidade = new Limetes(Result.Limites, map);
		    //}
		});
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
            RedeDemandas.push(new PontoEntregaMarker(Result.PontoEntregas[i], map));
        }
        /*  if (TipoFiltro == "NaSa") {
              MontaLegendaNaSa(quantNA, quantSA);
          }*/

        map.setZoom(14);
        //   if (Result.Centro.Lat && Result.Centro.Lon) {
        map.setCenter(new google.maps.LatLng(Result.Centro.Lat, Result.Centro.Lon));
        //  }
        HideAllModal();
    }

    //Add postes no map e no array de postes do java script
    function CreatePostesOs(Result) {
        //if (Result.Postes.length == 0) { bootbox.alert("Não existe Postes para esta consulta."); return; }
        var quantNA = 0;
        var quantSA = 0;
        for (var i = 0; i < Result.Postes.length; i++) {
            //caso um filtro esteja sendo aplicado os postes sao submetidos a esses metodos.
            if (TipoFiltro != undefined && TipoFiltro != "") {
                if (TipoFiltro == "Potencia") { Result.Postes[i].Img = AtribuiImgFiltroPotencia(Result.Postes[i].Img); }
                else if (TipoFiltro == "Lampada") { Result.Postes[i].Img = AtribuiImgFiltroLampada(Result.Postes[i].Img); }
                else if (TipoFiltro == "Status") { Result.Postes[i].Img = Result.Postes[i].Img; }
                else if (TipoFiltro == "NaSa") {
                    Result.Postes[i].Img = Result.Postes[i].Img;
                    if (Result.Postes[i].Img == "17") { quantNA++; }
                    else if (Result.Postes[i].Img == "19") { quantSA++; }
                }
            }
            RedePostes.push(new PosteMarker(Result.Postes[i], map));            
           // PostesId.push(new IdPosteIcon(Result.Postes[i], map));
        }
        if (TipoFiltro == "NaSa") {
            MontaLegendaNaSa(quantNA, quantSA);
        }

        map.setZoom(14);
        //  if (Result.Centro.Lat && Result.Centro.Lon) {
        map.setCenter(new google.maps.LatLng(Result.Centro.Lat, Result.Centro.Lon));
        //  }
        HideAllModal();
    }

    //carrega Postes de uma OS
    function PlotaVaosDemandasByOs(ordemServico) {
        CallServer("GET", "/Vao/GetVaosDemandasByOs", { codOs: ordemServico },
		function (Result) {
		    // if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
		    // ClearPontoEntrega();
		    TipoFiltro = "";
		    //LimpaLegenda();
		    //Id_Cidade = Result.Postes.IdCidade;
		    /*$("#labelCidade").text("Cidade: " + Result.Postes.NomeCidade);
		    $("#labelOs").show();
		    $("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));*/
		    if (Result.VaosDemandas != null) {
		        CreateVaosDemandas(Result.VaosDemandas);
		    }

		    //if (LimiteCidade != null) {
		    //    LimiteCidade.Remove();
		    //}
		    //if (Result.Limites != null) {
		    //    LimiteCidade = null;
		    //    LimiteCidade = new Limetes(Result.Limites, map);
		    //}
		});
    }

    //carrega Postes de uma OS
    function PlotaAnotacoesByOs(ordemServico) {
        CallServer("GET", "/Anotacoes/GetAnotacaoByOs", { codOs: ordemServico },
		function (Result) {
		    // if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
		    // ClearPontoEntrega();
		    TipoFiltro = "";
		    //LimpaLegenda();
		    //Id_Cidade = Result.Postes.IdCidade;
		    /*$("#labelCidade").text("Cidade: " + Result.Postes.NomeCidade);
		    $("#labelOs").show();
		    $("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));*/
		    CreateAnotacaoOs(Result);

		    /*if (LimiteCidade != null) {
		        LimiteCidade.Remove();
		    }
		    if (Result.Limites != null) {
		        LimiteCidade = null;
		        LimiteCidade = new Limetes(Result.Limites, map);
		    }*/
		});
    }

    //Criar os markers de Anotacoes
    function CreateAnotacaoOs(retorno) {
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
            RedeAnotacao.push(new AnotacaoMarkerOpcoes(retorno.Result[i], map));
        }
        /*  if (TipoFiltro == "NaSa") {
              MontaLegendaNaSa(quantNA, quantSA);
          }*/

        map.setZoom(14);
        /*if (Result.Centro.Lat && Result.Centro.Lon) { map.setCenter(new google.maps.LatLng(Result.Centro.Lat, Result.Centro.Lon)); }
        HideAllModal();*/
    }

    //carrega Postes de uma OS
    function PlotaStrandsByOs(ordemServico) {
        CallServer("GET", "/Strands/GetStrandsByOs", { codOs: ordemServico },
		function (Result) {
		    // if (Result.Msg != null) { bootbox.alert(Result.Msg); return; }
		    // ClearPontoEntrega();
		    TipoFiltro = "";
		    //LimpaLegenda();
		    //Id_Cidade = Result.Postes.IdCidade;
		    /*$("#labelCidade").text("Cidade: " + Result.Postes.NomeCidade);
		    $("#labelOs").show();
		    $("#labelOs").text("OS: " + $.GetParamUrl('PosteByOs'));*/
		    if (Result.Result != null) {
		        CreateStrandsOs(Result);
		    }

		    /*if (LimiteCidade != null) {
		        LimiteCidade.Remove();
		    }
		    if (Result.Limites != null) {
		        LimiteCidade = null;
		        LimiteCidade = new Limetes(Result.Limites, map);
		    }*/
		});
    }

    function CreateStrandsOs(retorno) {
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


    function EditarPontoEntrega() {

        limparCamposPontoEntrega();
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
                //$("#latitude_ponto_entrega").val(retorno.Latitude);
                //$("#longitude_ponto_entrega").val(retorno.Longitude);
                $("#numero_ponto_entrega").val(retorno.Numero);
                $("#codigo_geo_ponto_entrega").val(retorno.CodigoGeoBD);
                $("#complemento1_ponto_entrega").val(retorno.Complemento1);
                $("#id_ponto_entrega").val(retorno.IdPontoEntrega);
                $("#id_poste_ponto_entrega").val(retorno.IdPoste);
                $("#complemento_ponto_entrega").val(retorno.Complemento1);
                //$("#classificacao_ponto_entrega").val(retorno.Complemento1);
                //$("#descricao_poste").val(retorno.Descricao);

                $("#classe_social_ponto_entrega").val(retorno.ClasseSocial);
                $("#classificacao_ponto_entrega").val(retorno.Classificacao);
                $("#qtd_domicilio_ponto_entrega").val(retorno.QtdDomicilio);
                $("#domicilio_comercio_ponto_entrega").val(retorno.QtdDomicilioComercio);
                $("#qtd_andares_ponto_entrega").val(retorno.NumeroAndaresEdificio);
                $("#qtd_apartamentos_ponto_entrega").val(retorno.TotalApartamentosEdificio);
                $("#ocorrencia_ponto_entrega").val(retorno.Ocorrencia);
                $("#nome_edificio_ponto_entrega").val(retorno.NomeEdificio);
                $("#qtd_blocos_ponto_entrega").val(retorno.QtdBlocos);
                $("#tipo_comercio_ponto_entrega").val(retorno.ClassificacaoComercio);
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
			            conteudo_foto += '<span class="input-group-addon input-sm">DATA FOTO</span>';
			            conteudo_foto += '<input id="data_foto_' + identificador_foto + '" type="date" class="form-control input-sm" />';
			            conteudo_foto += '<span class="input-group-addon input-sm">NUMERO FOTO</span>';
			            conteudo_foto += '<input id="numero_foto_' + identificador_foto + '" type="text" class="form-control input-sm" maxlength="10"/>';
			            conteudo_foto += '<span class="input-group-btn"><button class="btn btn-danger btn-sm" type="button" onclick="GlobalMaps.get().RemoveFoto(' + identificador_foto + ');">EXCLUIR</button></span>';
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

    //salva as ediçoes na base de dados realizados no Ponto de Entrega
    function SalvarEdicaoPontoEntrega() {
        //	var medidores = [];
        var fotos = [];
        var erro = false;
        //dados do ponto de entrega

        var ClasseSocial = $("#classe_social_ponto_entrega :selected").val();
        var complemento = $("#complemento_ponto_entrega").val();
        //var Complemento1 = $("#complemento1_ponto_entrega_novo").val();
        // var Complemento2 = $("#complemento2_ponto_entrega_novo").val();
        var Obs = $("#observacao_ponto_entrega").val();
        var CodigoGeoBD = $("#codigo_geo_ponto_entrega").val();
        var Numero = $("#numero_ponto_entrega").val();
        //var x = NovaDemandaPin.getPosition().lat();
        //var y = NovaDemandaPin.getPosition().lng();

        var classificacao = $("#classificacao_ponto_entrega :selected").val();
        var tipoComercio = $("#tipo_comercio_ponto_entrega :selected").val();
        var ocorrencia = $("#ocorrencia_ponto_entrega :selected").val();
        var qtd_domicilio = $("#qtd_domicilio_ponto_entrega").val();
        var qtd_domicilio_comercio = $("#domicilio_comercio_ponto_entrega").val();
        var qtd_andares = $("#qtd_andares_ponto_entrega").val();
        var qtd_apartamentos = $("#qtd_apartamentos_ponto_entrega").val();
        var nome_edificio = $("#nome_edificio_ponto_entrega").val();
        var qtd_blocos = $("#qtd_blocos_ponto_entrega").val();



        /* var ponto_entrega = {
             IdPontoEntrega: $("#id_ponto_entrega").val(),
             IdPoste: $("#id_poste_ponto_entrega").val(),
             CodigoGeoBD: CodigoGeoBD,
             ClasseAtendimento: $("#classe_atendimento_ponto_entrega").val(),
             Numero: Numero,
             ClasseSocial: ClasseSocial,
             Complemento1: complemento,
             Observacao: Obs,*/
        //Latitude: $("#latitude_ponto_entrega").val().replace(/\./g, ','),
        //Longitude: $("#longitude_ponto_entrega").val().replace(/\./g, ','),
        //QuantidadeMedidores: 0,
        //Fase: $("#fase_ponto_entrega").val(),




        //   Medidores : medidores,
        //    EtLigacao : $("#et_ligacao_ponto_entrega").val(),
        //  TipoConstrucao : $("#tipo_construcao_ponto_entrega").val(),
        //  Status : $("#status_ponto_entrega").val(),
        // IdPoste : $("#id_poste_ponto_entrega").val(),
        //};
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

        for (var i = 0; i < identificadores_foto_ponto.length; i++) {
            numero_foto = $("#numero_foto_" + identificadores_foto_ponto[i]).val();
            data_foto = $("#data_foto_" + identificadores_foto_ponto[i]).val();

            if (numero_foto.length == 0 || data_foto.length == 0)
                erro = true;
        }

        if (!erro) {
            for (var i = 0; i < identificadores_foto_ponto.length; i++) {
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
            //ponto_entrega.QuantidadeMedidores = medidores.length           


            var demanda = {
                IdPontoEntrega: $("#id_ponto_entrega").val(),
                IdPoste: $("#id_poste_ponto_entrega").val(),
                IdCidade: $("#selectcityHome").val(),
                IdOrdemServicoTexto: $("#selectOrdemHome").val(),
                CodigoGeoBD: CodigoGeoBD,
                Numero: Numero,
                ClasseSocial: ClasseSocial,
                Complemento1: complemento,
                Observacao: Obs,
                // Complemento1:Complemento1,
                // Complemento2: Complemento2,
                //LatitudeTexto: x,
                //LongitudeTexto: y,
                //LatitudePosteTexto: latPoste,
                //LongitudePosteTexto: lonPoste,
                Classificacao: classificacao,
                Ocorrencia: ocorrencia,
                QtdDomicilio: qtd_domicilio,
                QtdDomicilioComercio: qtd_domicilio_comercio,
                NumeroAndaresEdificio: qtd_andares,
                TotalApartamentosEdificio: qtd_apartamentos,
                NomeEdificio: nome_edificio,
                QtdBlocos: qtd_blocos,
                ClassificacaoComercio: tipoComercio,
                Fotos: fotos

            };

            CallServer("POST", "/PontodeEntrega/SalvarPontoEntrega", { PontoEntregaReceived: demanda },
				function (response) {
				    VoltarOpcoesPoste();
				    bootbox.alert(response.Result);
				});
        }
        else { bootbox.alert("Não podemos prosseguir! Campo Numero ou Complemento não esta preenchido !") }
    }

    function ExcluirDemanda() {
        bootbox.confirm("<span class='glyphicon glyphicon-trash'></span> Deseja Realmente Exluir essa Demanda ?", function (result) {
            if (result) {
                HideAllModal();

                var indiceArrayDemanda;
                var indiceArrayVao
                var idDemandaClicado = $("#hiden_idDemanda").attr("value");
                for (var i = 0; i < RedeDemandas.length; i++) {
                    if (RedeDemandas[i].IdPontoEntrega == idDemandaClicado) {
                        indiceArrayDemanda = i;
                    }
                }

                for (var i = 0; i < RedeVaosDemanda.length; i++) {
                    if (RedeVaosDemanda[i].ID == idDemandaClicado) {
                        indiceArrayVao = i;
                    }
                }

                CallServer("POST", "/AjaxRede/ExcluirDemanda", { idDemanda: idDemandaClicado }, function (r) {
                    if (r.Status == 2) {
                        RedeDemandas[indiceArrayDemanda].Remove();
                        RedeDemandas.splice[indiceArrayDemanda, 1];

                        RedeVaosDemanda[indiceArrayVao].Remove();
                        RedeVaosDemanda.splice[indiceArrayVao, 1];

                    }
                    //bootbox.alert(r.Result);
                });
            }
        });
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

    //Fecha os outros modais e Carrega o modal com as Opçoes do poste
    function VoltarOpcoesPoste() {
        //fecha todos os modal da pagina.
        HideAllModal();
        //abri o menu do poste com as opçoes refente ao mesmo.
        // MontarBodyModalOpcoes();
    }
    function LimpaFormPontoEntrega() {
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

    function CancelarArrastarPoste() {
        markerArrastado.setPosition(new google.maps.LatLng(latPosteAntigo, lonPosteAntigo));
        HideAllModal();
    }

    function SalvarArrastarPoste() {
        $("#carregaOrdem").css("display", "block");
        var requestData = {
            //IdCidade: Id_Cidade,
            IdPoste: idPosteClickado1,
            Latitude: latPoste,
            Longitude: lonPoste,
        };

        $.ajax({
            url: '/AjaxRede/SalvarArrastarPoste',
            type: 'POST',
            data: JSON.stringify(requestData),
            cache: false,
            headers: { '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
            dataType: 'json',
            contentType: 'application/json',
            error: function (xhr) {
                bootbox.alert('Error: ' + xhr.statusText);
                //abri o menu do poste com as opçoes refente ao mesmo.
                //VoltarOpcoesPoste();
            },
            success: function (r) {
                if (r.Poste != null) {
                    for (var p = 0; p < RedePostes.length; p++) {
                        if (RedePostes[p].IdPoste == r.Poste.IdPoste) {
                            RedePostes[p].Remove();
                            RedePostes.splice(p, 1);
                            RedePostes.push(new PosteMarker(r.Poste, map));
                            break;
                        }
                    }
                    for (var i = 0; i < r.VaosDemandas.VaosDemandas.length; i++) {
                        for (var j = 0; j < RedeVaosDemanda.length; j++) {
                            if (r.VaosDemandas.VaosDemandas[i].ID == RedeVaosDemanda[j].ID) {
                                RedeVaosDemanda[j].Remove();
                                RedeVaosDemanda.splice(j, 1);
                                RedeVaosDemanda.push(new VaoDemandasHome(r.VaosDemandas.VaosDemandas[i], map));
                                break;
                            }
                        }
                    }

                    //bootbox.alert("Sucesso");
                    //abri o menu do poste com as opçoes refente ao mesmo.
                    // VoltarOpcoesPoste();
                    HideAllModal();
                    idPosteClickado1 = 0;
                    latPoste = 0;
                    lonPoste = 0;
                    $("#carregaOrdem").css("display", "none");
                } else { bootbox.alert("Erro : " + r.ErroMsg); }
            },
            async: true,
            processData: false
        });
    }
    function EditarPoste() {

        limpaCamposEditPoste();
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
             //   $("#descricao_poste").val(retorno.Descricao);
               
                $("#encontrado_poste").val(retorno.EncontradoPoste);
                $("#para_raio_poste").val(retorno.PararioPoste);
                $("#aterramento_poste").val(retorno.AterramentoPoste);
                $("#estrutura_primaria_poste").val(retorno.EstruturaPrimariaPoste);
                $("#estrutura_secundaria_poste").val(retorno.EstruturaSecundaria_poste);
                $("#quantidade_estai").val(retorno.QuantidadeEstai);
                $("#ano_poste").val(retorno.AnoPoste);
                $("#situacao_poste").val(retorno.SituacaoPoste);
                $("#equipamento_poste").val(retorno.EquipamentoPoste);
                $("#mufla_poste").val(retorno.MuflaPoste);
                $("#rede_primario_poste").val(retorno.RedePrimarioPoste);
                $("#defeito_poste").val(retorno.DefeitoPoste);
                $("#barramento_poste").val(retorno.BarramentoPoste);
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
                        conteudo_foto += '<span class="input-group-addon input-sm">DATA FOTO</span>';
                        conteudo_foto += '<input id="data_foto_' + identificador_foto + '" type="date" class="form-control input-sm" />';
                        conteudo_foto += '<span class="input-group-addon input-sm">NUMERO FOTO</span>';
                        conteudo_foto += '<input id="numero_foto_' + identificador_foto + '" type="text" class="form-control input-sm" maxlength="10"/>';
                        conteudo_foto += '<span class="input-group-btn"><button class="btn btn-danger btn-sm" type="button" onclick="GlobalMaps.get().RemoveFoto(' + identificador_foto + ');">EXCLUIR</button></span>';
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

    function RemoveFoto(identificador) {
        var indexObj;
        for (var i = 0; i < identificadores_foto.length; i++) {
            if (identificadores_foto[i] == identificador.id) {
                indexObj = i;
            }
        }
        identificadores_foto.splice(indexObj, 1);//remove do array				        
        $(identificador).remove();
    }

    //Salva um novo Poste no Base
    function SalvarNovoPoste(NovoMaker) {

        var Os = $('#selectOrdemHome :selected').val();

        //CallServer("POST", "/AjaxOrdemDeServico/NewPoste", {
        //    IdCidadeview: IdCidade,
        //    IdOrdemDeServicoview: Os
        //        },
        //    function (r) {
        //        if (r.Msg == "OK") {
        //            $('#CriarOS').modal('hide');
        //        }
        //    });

        //Salva um novo Poste no Base

        // var Os = $('#select-os').val();
        if (Os != "") {
            var requestData = {
                IdCidade: Id_Cidade,
                Latitude: NovoMaker.getPosition().lat(),
                Longitude: NovoMaker.getPosition().lng(),
                IdOrdemServicoTexto: Os,
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
                        RedePostes.push(new PosteMarker(r.Result, map))
                        NovoMaker.setMap(null);
                        //fecha todos os modal da pagina
                        HideAllModal();
                        //  bootbox.alert("Sucesso");
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
                    //bootbox.alert(r.Result);
                });
            }
        });
    }

    //Salva um novo Poste no Base
    function SalvarNovaAnotacao() {
        var idAnotacao = $("#hiden_idAnotocao").val();
        if (idAnotacao != "") {
            SalvarNovaAnotacaoEditado(idAnotacao);
        } else {

            var Os = $('#selectOrdemHome :selected').val();

            //CallServer("POST", "/AjaxOrdemDeServico/NewPoste", {
            //    IdCidadeview: IdCidade,
            //    IdOrdemDeServicoview: Os
            //        },
            //    function (r) {
            //        if (r.Msg == "OK") {
            //            $('#CriarOS').modal('hide');
            //        }
            //    });

            //Salva um novo Poste no Base

            // var Os = $('#select-os').val();

            var descricao = $("#descricaoAnotacao").val();
            if (descricao == "") {
                alert("Insira a Descrição");
                return;
            }
            if (Os != "") {
                var requestData = {
                    IdCidade: Id_Cidade,
                    X: NovaAnotacao.getPosition().lat(),
                    Y: NovaAnotacao.getPosition().lng(),
                    NumeroOs: Os,
                    Descricao: descricao
                };

                $.ajax({
                    url: '/AjaxRede/NewAnotacao',
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
                            RedeAnotacao.push(new AnotacaoMarkerOpcoes(r.Result, map));
                            NovaAnotacao.setMap(null);
                            //fecha todos os modal da pagina
                            HideAllModal();
                            //  bootbox.alert("Sucesso");
                            DesSelecionarAnotacao();
                            $("#hiden_idAnotocao").val("");
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
    }

    //Salva um novo Poste no Base
    function SalvarNovaAnotacaoEditado(idAnotacao) {

        //var Os = $('#selectOrdemHome :selected').val();

        //CallServer("POST", "/AjaxOrdemDeServico/NewPoste", {
        //    IdCidadeview: IdCidade,
        //    IdOrdemDeServicoview: Os
        //        },
        //    function (r) {
        //        if (r.Msg == "OK") {
        //            $('#CriarOS').modal('hide');
        //        }
        //    });

        //Salva um novo Poste no Base

        // var Os = $('#select-os').val();

        var descricao = $("#descricaoAnotacao").val();
        //var idAnotacao = $("#hiden_idAnotocao").val();
        var idAnotacaoLong = parseInt(idAnotacao);

        CallServer("POST", "Anotacoes/EditarAnotacao", { id: idAnotacao, descricao: descricao }, function (Result) {
            if (Result.Result == "OK") {
                for (var i = 0; i < RedeAnotacao.length; i++) {
                    if (RedeAnotacao[i].IdAnotacao == idAnotacaoLong) {
                        RedeAnotacao[i].marker.setTitle(descricao);
                        HideAllModal();
                    }
                }
            } else {
                bootbox.alert("Não podemos proseeguir! Anotação Não Encotrada!")
            }
        });
    }

    //set o dh_exlusao na base e retira do mapa o seguinte anotacao
    function ExcluirAnotacao() {
        bootbox.confirm("<span class='glyphicon glyphicon-trash'></span> Deseja Realmente Exluir essa Anotaçao?", function (result) {
            if (result) {
                //fecha todos os madal da pagina.
                HideAllModal();
                //index do obj desejado na array
                var indexObj;
                //poste Clicado.
                $("#carregaOrdem").css("display", "block");
                var idAnotacaoClick = $('#hiden_idAnotocao').attr("value");
                for (var i = 0; i < RedeAnotacao.length; i++) {
                    if (RedeAnotacao[i].IdAnotacao == idAnotacaoClick) {
                        indexObj = i;
                    }
                }
                CallServer("POST", "/Anotacoes/ExcluirAnotacao", { idAnotacao: idAnotacaoClick },
                function (r) {
                    if (r.Status == 2) {
                        RedeAnotacao[indexObj].Remove();//remove do mapa
                        RedeAnotacao.splice(indexObj, 1);//remove do array		
                        $("#carregaOrdem").css("display", "none");
                    }
                    //bootbox.alert(r.Result);
                });
            }
        });
    }

    function limpaCamposEditPoste() {
        //$('#hiden_idPoste').attr("value").val("");
        $("#altura_poste").val("");
        $("#esforco_poste").val("");
        $("#tipo_poste").val("");
        $("#encontrado_poste").val("");
        $("#para_raio_poste").val("");
        $("#aterramento_poste").val("");
        $("#estrutura_primaria_poste").val("");
        $("#estrutura_secundaria_poste").val("");
        $("#quantidade_estai").val("");
        $("#ano_poste").val("");
        $("#situacao_poste").val("");
        $("#equipamento_poste").val("");
        $("#mufla_poste").val("");
        $("#rede_primario_poste").val("");
        $("#defeito_poste").val("");
    }

    function SalvarEditPoste() {
        /// Processamento Foto

        var numero_foto, data_foto;
        var erro = false;

        for (var i = 0; i < identificadores_foto.length; i++) {
            numero_foto = $("#numero_foto_" + identificadores_foto[i]).val();
            data_foto = $("#data_foto_" + identificadores_foto[i]).val();

            if (numero_foto.length == 0 || data_foto.length == 0)
                erro = true;
        }

        if (!erro) {
            fotos_objetos = [];
            for (var i = 0; i < identificadores_foto.length; i++) {
                var foto;
                numero_foto = $("#numero_foto_" + identificadores_foto[i]).val();
                data_foto = $("#data_foto_" + identificadores_foto[i]).val();
                foto = { NumeroFoto: numero_foto, DataFoto: data_foto };
                fotos_objetos.push(foto);
            }
        }

        /// Variaveis dos Dados do Poste
        var Id_Poste = $('#hiden_idPoste').attr("value");

        var altura_poste = $("#altura_poste").val().toString().replace(/\./g, ',');
        var esforco_poste = $("#esforco_poste").val();
        var tipoposte_poste = $("#tipo_poste").val();
        var encontrado_poste = $("#encontrado_poste").val();
        var barramento_poste = $("#barramento_poste").val();
        var parario_poste = $("#para_raio_poste").val();
        var aterramento_poste = $("#aterramento_poste").val();
        var estrutura_primaria_poste = $("#estrutura_primaria_poste").val();
        var estrutura_secundaria_poste = $("#estrutura_secundaria_poste").val();
        var quantidade_estai = $("#quantidade_estai").val();
        var ano_poste = $("#ano_poste").val();
        var situacao_poste = $("#situacao_poste").val();
        var equipamento_poste = $("#equipamento_poste").val();
        var mufla_poste = $("#mufla_poste").val();
        var rede_primario_poste = $("#rede_primario_poste").val();
        var defeito_poste = $("#defeito_poste").val();

        var fotos = $("#numfotos").val();
        //var defeito_poste = $("#defeito_poste").val().toString().replace(/\./g, ',');


        //var descricao = $("#descricao_poste").val();
        var data_diretotio = $("#data_diretotio_poste").val();
        //var lat = $("#input_Latitude").val().replace(/\./g, ',');
        //var lon = $("#input_Longitude").val().replace(/\./g, ',');

        var requestData = {
            IdCidade: Id_Cidade,
            IdPoste: Id_Poste,
            LstFoto: fotos_objetos,
            Altura: altura_poste,
            TipoPoste: tipoposte_poste,
            Esforco: esforco_poste,
            EncontradoPoste: encontrado_poste,
            BarramentoPoste: barramento_poste,
            PararioPoste: parario_poste,
            AterramentoPoste: aterramento_poste,
            EstruturaPrimariaPoste: estrutura_primaria_poste,
            EstruturaSecundaria_poste: estrutura_secundaria_poste,
            QuantidadeEstai: quantidade_estai,
            AnoPoste: ano_poste,
            SituacaoPoste: situacao_poste,
            EquipamentoPoste: equipamento_poste,
            MuflaPoste: mufla_poste,
            RedePrimarioPoste: rede_primario_poste,
            DefeitoPoste: defeito_poste

            //Descricao: descricao,
            // Latitude: lat,
            // Longitude: lon,
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

    //Carrega novamente as informçoes do poste, que estao na base de dados.
    function CancelarEdicaoPoste() {
        ////id poste clicado
        //var idPosteclicado = $('#hiden_idPoste').attr("value");
        ////index do obj no array
        //var indexObj;
        ////fecha todos os modal da pagina
        //HideAllModal();
        //for (var i = 0; i < RedePostes.length; i++) { if (RedePostes[i].IdPoste == idPosteclicado) { indexObj = i; } }
        //CallServer("GET", "/AjaxRede/GetPosteById", { IdPoste: idPosteclicado, IdCidade: Id_Cidade },
        //function (retorno) {
        //    //remove do mapa
        //    RedePostes[indexObj].Remove();
        //    //remove do array
        //    RedePostes.splice(indexObj, 1);
        //    //add o obj no array
        //    RedePostes.push(new PosteMarker(retorno, map));
        //});

        HideAllModal();
    }



    function limparCamposPontoEntrega() {
        $("#classe_social_ponto_entrega :selected").val("");
        $("#complemento_ponto_entrega").val("");
        $("#complemento1_ponto_entrega_novo").val("");
        // var Complemento2 = $("#complemento2_ponto_entrega_novo").val();
        $("#observacao_ponto_entrega").val();
        var CodigoGeoBD = $("#codigo_geo_ponto_entrega").val("");
        var Numero = $("#numero_ponto_entrega").val("");
        //var x = NovaDemandaPin.getPosition().lat();
        //var y = NovaDemandaPin.getPosition().lng();

        $("#classificacao_ponto_entrega :selected").val(1);
        $("#tipo_comercio_ponto_entrega :selected").val(1);
        $("#ocorrencia_ponto_entrega :selected").val(1);
        $("#qtd_domicilio_ponto_entrega").val("");
        $("#domicilio_comercio_ponto_entrega").val("");
        $("#qtd_andares_ponto_entrega").val("");
        $("#qtd_apartamentos_ponto_entrega").val("");
        $("#nome_edificio_ponto_entrega").val("");
        $("#qtd_blocos_ponto_entrega").val("");
    }

    function CancelarEdicaoPontoEntrega() {
        limparCamposPontoEntrega();
        HideAllModal();
    }

    function ApagarStrand() {
        var hiden_idStrands = $("#hiden_idStrands").val();

        var indexObj;
        for (var i = 0; i < RedeStrands.length; i++) {
            if (RedeStrands[i].ID == hiden_idStrands) {
                indexObj = i;
            }
        }
        $("#carregaOrdem").css("display", "block");
        CallServer("DELETE", "/Strands/ExcluirStrands", { id: hiden_idStrands }, function (Result) {
            if (Result = "OK") {
                RedeStrands[indexObj].Remove();//remove do mapa
                RedeStrands.splice(indexObj, 1);//remove do array		
                $("#carregaOrdem").css("display", "none");
            }
        });

        HideAllModal();
    }


    function SalvarNovaDemanda() {

        var ClasseSocial = $("#classe_social_ponto_entrega_novo :selected").val();
        var complemento = $("#complemento_ponto_entrega_novo").val();
        //var Complemento1 = $("#complemento1_ponto_entrega_novo").val();
        // var Complemento2 = $("#complemento2_ponto_entrega_novo").val();
        var Obs = $("#observacao_ponto_entrega_novo").val();
        var CodigoGeoBD = $("#codigo_geo_ponto_entrega_novo").val();
        var Numero = $("#numero_ponto_entrega_novo").val();
        var x = NovaDemandaPin.getPosition().lat();
        var y = NovaDemandaPin.getPosition().lng();

        var classificacao = $("#classificacao_ponto_entrega_novo :selected").val();
        var tipoComercio = $("#tipo_comercio_ponto_entrega_novo :selected").val();
        var ocorrencia = $("#ocorrencia_ponto_entrega_novo :selected").val();
        var qtd_domicilio = $("#qtd_domicilio_ponto_entrega_novo").val();
        var qtd_domicilio_comercio = $("#domicilio_comercio_ponto_entrega_novo").val();
        var qtd_andares = $("#qtd_andares_ponto_entrega_novo").val();
        var qtd_apartamentos = $("#qtd_apartamentos_ponto_entrega_novo").val();
        var nome_edificio = $("#nome_edificio_ponto_entrega_novo").val();
        var qtd_blocos = $("#qtd_blocos_ponto_entrega_novo").val();


        var fotos = [];

        var Os = $('#selectOrdemHome :selected').val();
        var idCidade = $('#selectcityHome :selected').val();
        var demanda = {
            IdPoste: idPosteClickado,
            IdCidade: idCidade,
            IdOrdemServicoTexto: Os,
            CodigoGeoBD: CodigoGeoBD,
            Numero: Numero,
            ClasseSocial: ClasseSocial,
            Complemento1: complemento,
            Observacao: Obs,
            // Complemento1:Complemento1,
            // Complemento2: Complemento2,
            LatitudeTexto: x,
            LongitudeTexto: y,
            LatitudePosteTexto: latPoste,
            LongitudePosteTexto: lonPoste,
            Classificacao: classificacao,
            Ocorrencia: ocorrencia,
            QtdDomicilio: qtd_domicilio,
            QtdDomicilioComercio: qtd_domicilio_comercio,
            NumeroAndaresEdificio: qtd_andares,
            TotalApartamentosEdificio: qtd_apartamentos,
            NomeEdificio: nome_edificio,
            QtdBlocos: qtd_blocos,
            ClassificacaoComercio: tipoComercio

        };


        CallServer("POST", "/PontodeEntrega/SalvarPontoEntregaNovo", { PontoEntregaReceived: demanda }, function (Result) {
            if (Result != null) {
                RedeDemandas.push(new PontoEntregaMarker(Result.Demanda, map));
                if (Result.VaoDemandaPoste != null) {
                    RedeVaosDemanda.push(new VaoDemandasHome(Result.VaoDemandaPoste, map));
                }
                NovaDemandaPin.setMap(null);
                HideAllModal();
                DesSelecionaDemanda();

                var ClasseSocial = $("#classe_social_ponto_entrega_novo :selected").val();
                var complemento = $("#complemento_ponto_entrega_novo").val("");
                //var Complemento1 = $("#complemento1_ponto_entrega_novo").val();
                // var Complemento2 = $("#complemento2_ponto_entrega_novo").val();
                var Obs = $("#observacao_ponto_entrega_novo").val("");
                var CodigoGeoBD = $("#codigo_geo_ponto_entrega_novo").val("");
                var Numero = $("#numero_ponto_entrega_novo").val("");

                //var classificacao = $("#classificacao_ponto_entrega_novo :selected").val();
                //var tipoComercio = $("#tipo_comercio_ponto_entrega_novo :selected").val();
                //var ocorrencia = $("#ocorrencia_ponto_entrega_novo :selected").val();
                var qtd_domicilio = $("#qtd_domicilio_ponto_entrega_novo").val("");
                var qtd_domicilio_comercio = $("#domicilio_comercio_ponto_entrega_novo").val("");
                var qtd_andares = $("#qtd_andares_ponto_entrega_novo").val("");
                var qtd_apartamentos = $("#qtd_apartamentos_ponto_entrega_novo").val("");
                var nome_edificio = $("#nome_edificio_ponto_entrega_novo").val("");
                var qtd_blocos = $("#qtd_blocos_ponto_entrega_novo").val("");
            }
        });
    }

    function CriarNovoStrand() {

        var idCidade = $("#selectcityHome").val();
        var IdOrdemServico = $("#selectOrdemHome").val();

        var demandaStrandView = {
            IdCidade: idCidade,
            NumeroOS: IdOrdemServico,
            IdPoste1: idPosteClickado,
            IdPoste2: idPosteClickado1
        };

        $("#carregaOrdem").css("display", "block");

        CallServer("POST", "Strands/Novo", { demandaStrandView: demandaStrandView }, function (Result) {
            if (Result != null) {
                RedeStrands.push(new Strands(Result.DemandaStrand, map));
                idPosteClickado = 0;
                DesSelecionaStrand();
                $("#carregaOrdem").css("display", "none");
            }
        });
    }

    function CriarNovoStrandNoMapa() {

        var idCidade = $("#selectcityHome").val();
        var IdOrdemServico = $("#selectOrdemHome").val();

        var demandaStrandView = {
            IdCidade: idCidade,
            NumeroOS: IdOrdemServico,
            X1Texto: latStrand1,
            Y1Texto: lonStrand1,
            X2Texto: latStrand2,
            Y2Texto: lonStrand2
        };

        $("#carregaOrdem").css("display", "block");

        CallServer("POST", "Strands/NovoNoMaps", { demandaStrandView: demandaStrandView }, function (Result) {
            if (Result != null) {
                RedeStrands.push(new Strands(Result.DemandaStrand, map));
                idPosteClickado = 0;
                DesSelecionaStrand();
                $("#carregaOrdem").css("display", "none");
                latStrand1 = 0;
            }
        });
    }

    function EditarAnotacao() {
        $("#ModalDescricaoAnotacao").modal('show');
    }

    function AddFoto() {

        cont_foto++;

        var identificador_foto = 'foto_' + cont_foto;

        var conteudo_foto = '<div class="input-group" id="' + identificador_foto + '">';
        conteudo_foto += '<span class="input-group-addon input-sm">DATA FOTO</span>';
        conteudo_foto += '<input id="data_foto_' + identificador_foto + '" type="date" class="form-control input-sm" />';
        conteudo_foto += '<span class="input-group-addon input-sm">NUMERO FOTO</span>';
        conteudo_foto += '<input id="numero_foto_' + identificador_foto + '" type="text" class="form-control input-sm" maxlength="10"/>';
        conteudo_foto += '<span class="input-group-btn"><button class="btn btn-danger btn-sm" type="button" onclick="GlobalMaps.get().RemoveFoto(' + identificador_foto + ');">EXCLUIR</button></span>';
        conteudo_foto += '</div>';
        conteudo_foto += '<p></p>';

        $("#div_fotos").append(conteudo_foto);

        identificadores_foto.push(identificador_foto);
    }

    function InicioLocaluser() {
        setInterval(function(){ 
            PegaLocalizacaoUser();
        }, 3000);
    }

    function PegaLocalizacaoUser() {
      /*  CallServer("GET", "/LocalUserWeb/PegarLocalUser", {}, function (Result) {
            if (Result != null) {
                $.each(Result.LocaisUsers, function (i, obj) {
                    Localusers.push(new LocalUserHome(obj, map));
                });
            }
        });*/
        $.ajax({
            url: '/LocalUserWeb/PegarLocalUser',
            type: 'GET',
            //data: JSON.stringify(requestData),
            cache: false,
            headers: { '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
            dataType: 'json',
            contentType: 'application/json',
            error: function (xhr) {
            },
            success: function (Result) {
                if (Result != null) {
                    $.each(Result.LocaisUsers, function (i, obj) {                        
                        if (Localusers.has(obj.NomeUser)) {                       
                            
                            Localusers.get(obj.NomeUser).marker.setPosition(new google.maps.LatLng(obj.X, obj.Y));
                            Localusers.get(obj.NomeUser).markerIcon.setPosition(new google.maps.LatLng(obj.X, obj.Y));
                            Localusers.get(obj.NomeUser).markerIcon.setTitle("Ultimo Acesso: " + obj.Time);
                            if (obj.Panico == 1) {
                                Localusers.get(obj.NomeUser).markerIcon.setAnimation(google.maps.Animation.BOUNCE);
                                Localusers.get(obj.NomeUser).markerIcon.setIcon("Images/personajuda.png");
                                //Localusers.get(obj.NomeUser).marker.setColo(new google.maps.LatLng(obj.X, obj.Y));
                            } else {
                                Localusers.get(obj.NomeUser).markerIcon.setAnimation(null);
                                Localusers.get(obj.NomeUser).markerIcon.setIcon("Images/person.png");
                            }
                        }else{
                            Localusers.set(obj.NomeUser, new LocalUserHome(obj, map));
                        }
                     //   Localusers.push(new LocalUserHome(obj, map));                              
                      /*  $.each(Localusers, function (i, objExistente) {
                            if (obj.NomeUser == objExistente.NomeUser) {
                                objExistente.marker.setPosition(new google.maps.LatLng(obj.X, obj.Y));
                            } else {
                                Localusers.push(new LocalUserHome(obj, map));
                            }
                        });*/
                    });

                } else { bootbox.alert("Erro : " + r.ErroMsg); }
            },
            async: true,
            processData: false
        });
    }

    function AddFotoPontoEntrega() {

        cont_foto_ponto++;

        var identificador_foto_ponto = 'foto_' + cont_foto_ponto;

        var conteudo_foto_ponto = '<div class="input-group input-sm" id="' + identificador_foto_ponto + '">';
        conteudo_foto_ponto += '<span class="input-group-addon input-sm">DATA FOTO</span>';
        conteudo_foto_ponto += '<input id="data_foto_' + identificador_foto_ponto + '" type="date" class="form-control input-sm" />';
        conteudo_foto_ponto += '<span class="input-group-addon input-sm">NUMERO FOTO</span>';
        conteudo_foto_ponto += '<input id="numero_foto_' + identificador_foto_ponto + '" type="text" class="form-control input-sm" maxlength="10"/>';
        conteudo_foto_ponto += '<span class="input-group-btn"><button class="btn btn-danger btn-sm" type="button" onclick="GlobalMaps.get().RemoveFotoPontoEntrega(' + identificador_foto_ponto + ');">EXCLUIR</button></span>';
        conteudo_foto_ponto += '</div>';
        conteudo_foto_ponto += '<p></p>';

        $("#div_foto_ponto_entrega").append(conteudo_foto_ponto);

        identificadores_foto_ponto.push(identificador_foto_ponto);
    }
    function RemoveFotoPontoEntrega(identificador) {
        var indexObj;
        for (var i = 0; i < identificadores_foto_ponto.length; i++) {
            if (identificadores_foto_ponto[i] == identificador.id) {
                indexObj = i;
            }
        }
        identificadores_foto_ponto.splice(indexObj, 1);//remove do array				        
        $(identificador).remove();
    }

    function NovaCidade() {
        //$("#modalnovacidade").modal('show');
        if(novaCidade == true){
           
            $("#caixaNovaCidade").css('display', 'block');
            $("#caixaEscolheCidade").css('display', 'none'); 
            $("#novaCidade").removeClass('btn-primary');
            $("#novaCidade").addClass('btn-warning');
            novaCidade = false;
        } else {
            $("#caixaNovaCidade").css('display', 'none');
            $("#novaCidade").removeClass('btn-warning');
            $("#novaCidade").addClass('btn-primary');
            $("#caixaEscolheCidade").css('display', 'block'); 
            novaCidade = true;
        }
                
    }

    function geolocate() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var geolocation = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                var circle = new google.maps.Circle({
                    center: geolocation,
                    radius: position.coords.accuracy
                });
                autocomplete.setBounds(circle.getBounds());
            });
        }
    }


    function initAutocompleteRua() {
        // Create the autocomplete object, restricting the search to geographical
        // location types.
        autocomplete = new google.maps.places.Autocomplete(
            /** @type {!HTMLInputElement} */(document.getElementById('nomeRua')),
            { types: ['geocode'] });

        // When the user selects an address from the dropdown, populate the address
        // fields in the form.
        //autocomplete.addListener('place_changed', fillInAddress);
    }

    function initAutocomplete() {       
        // Create the autocomplete object, restricting the search to geographical
        // location types.
        autocomplete = new google.maps.places.Autocomplete(
            /** @type {!HTMLInputElement} */(document.getElementById('nomeCidade')),
            { types: ['geocode'] });

        // When the user selects an address from the dropdown, populate the address
        // fields in the form.
        autocomplete.addListener('place_changed', fillInAddress);
    }

    function fillInAddress() {
        // Get the place details from the autocomplete object.
        var place = autocomplete.getPlace();

        for (var component in componentForm) {
            document.getElementById(component).value = '';
            document.getElementById(component).disabled = false;
        }        
        // Get each component of the address from the place details
        // and fill the corresponding field on the form.
        for (var i = 0; i < place.address_components.length; i++) {
            var addressType = place.address_components[i].types[0];
            if (componentForm[addressType]) {
                var val = place.address_components[i][componentForm[addressType]];
                document.getElementById(addressType).value = val;
            }
        }
    }

    function CadastrarCidade() {

        var nome = $("#nomeCidade").val().split(",");
        var zona = $("#zona").val();
        var setimodigito = $("#setimodigito").val();
        var norteSul = $("#norteSul :selected").val();

        var cidade = {
            Nome: nome[0],
            NorteOuSul: norteSul,
            Zona: zona,
            SetimoDigito: setimodigito,
        };

        if(nome == ""){
            $("#nomeCidadeDiv").addClass("has-error")
        } else if (zona == "") {
            $("#zonaDiv").addClass("has-error")
        } else if (setimodigito == "") {
            $("#setimodigitoDiv").addClass("has-error")
        } else if (norteSul == "") {
            $("#norteSulDiv").addClass("has-error")
        } else {

            $("#nomeCidadeDiv").removeClass("has-error")
            $("#zonaDiv").removeClass("has-error")
            $("#setimodigitoDiv").removeClass("has-error")
            $("#norteSulDiv").removeClass("has-error")

            CallServer("POST", "/AjaxOrdemDeServico/CadastrarCidade", { cidade: cidade }, function (result) {
                if (result.Result == "OK") {
                    PreencheHtmlDOMCidadesHome();
                    $("#caixaNovaCidade").css('display', 'none');
                    $("#novaCidade").removeClass('btn-warning');
                    $("#novaCidade").addClass('btn-primary');
                    $("#caixaEscolheCidade").css('display', 'block');
                    novaCidade = true;
                }
            });
        }
    }

    function codeAddress() {
        //var address = document.getElementById('selectcityHome').value;
        var address = $("#selectcityHome :selected").text();
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status == 'OK') {
                map.setCenter(results[0].geometry.location);
               /* var marker = new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location
                });*/
                boundsRect.setBounds(results[0].geometry.bounds);
                boundsRect.setMap(map);
            } else {
                alert('Geocode was not successful for the following reason: ' + status);
            }
        });
    }

    function LimparCampRua() {
        $("#nomeRua").val('');
    }

    function ProcurarRua() {
        codeAddressRua();
    }

    function codeAddressRua() {
        //var address = document.getElementById('selectcityHome').value;
        var addressRua = $("#nomeRua").val();
        if (addressRua != "") {
            $("#campoSearchRua").removeClass('has-error');
            $("#errorua").text('');
        var addressRualabel = $("#nomeRua").val().split("-");
        geocoder.geocode({ 'address': addressRua }, function (results, status) {
            if (status == 'OK') {
                //map.setCenter(results[0].geometry.location);
                /* var marker = new google.maps.Marker({
                     map: map,
                     position: results[0].geometry.location
                 });*/
                /*boundsRect.setBounds(results[0].geometry.bounds);
                boundsRect.setMap(map);*/

                var NovoMaker = new google.maps.Marker({
                    position: results[0].geometry.location,
                    map: map,
                    label: {
                        color: 'red',
                        fontWeight: 'bold',
                        text: '' + addressRualabel[0]
                    },                    
                    icon: {                        
                        labelOrigin: new google.maps.Point(14, -11),
                        url: 'Images/01.png'
                    }
                });

            } else {
                alert('Geocode was not successful for the following reason: ' + status);
            }
        });
        } else {
            $("#campoSearchRua").addClass('has-error');
            $("#errorua").text('Digite o nome de uma rua!');
        }
    }

    function FechaModal() {
        HideAllModal();
    }

    function MontarModalAnotacao() {
        $("#ModalOpcoesAnotacao").modal('show');
    }

    function MontarModalStrands() {
        $("#ModalOpcoesStrands").modal('show');
    }

    function RemoveAllButtonModalOpcoes() {
        //Remove os Botoes do Modal 
        $("#opcoesModal").empty();
    }

    function LimpaLegenda() {
        $('#panelLegenda').empty();
    }
    //fecha todos os madal da pagina
    function HideAllModal() {
        //fecha todos os modais pela classe
        $('.modal').modal('hide');
    }

    function BarradeProgresso(porcentagem) {
        var conteudo = '<div class="progress-bar" role="progressbar" aria-valuenow="' + porcentagem + '" aria-valuemin="0" aria-valuemax="100" style="width: ' + porcentagem + '%;"> ' + porcentagem + '% Finalizado </div>';
        $("#barradeprogresso").empty();
        $("#barradeprogresso").append(conteudo);
    }
    function init() {
        if (typeof (jQuery) === 'undefined') { alert("O jQuery não foi carregado. Verifique o import da libs."); return; }
        if (!(typeof google === 'object' && typeof google.maps === 'object')) { alert("O GoogleMaps API não foi carregado. Verifique o import da libs."); return; }
        return {
            StartMap: function () { LoadMap(); },
            CarregaCidade: function (Id_Cidade) { CarregaCidade(Id_Cidade); },
            Refresh: function () { Refresh(); },
            CarregarOSCidade: function () { PreencheHtmlDOMCidades(); },
            PlotarOsMapa: function () { PlotOsByCidade(); },
            LoadPostesByOs: function () { LoadPostesByOs(); },
            EditarOs: function () { EditarOsView(); },
            CriarOs: function () { CriarOsView(); },
            MontaBarradeProgresso: function (porcentagem) { BarradeProgresso(porcentagem); },
            ConcelarCricaoOs: function () { ConcelarCricaoOs(); },
            ApagarOrdem: function () { ApagarOrdem(); },
            PlotaPostesByCidade: function () { PlotaPostesByCidade() },
            verificaDentroPostePolygon: function () { verificaDentroPostePolygon() },
            PlotaDemandasByCidade: function () { PlotaDemandasByCidade() },
            ClearPostes: function () { ClearPostes() },
            PlotaStrandsByCidade: function () { PlotaStrandsByCidade() },
            PlotaAnotacoesByOs: function () { PlotaAnotacoesByOs() },
            MontarModalPontoOpcoes: function () { MontarBodyModalPontoEntregaOpcoes() },
            CarregaOrdemPorCidade: function () { CarregaOrdemPorCidade() },
            EditarPontoEntrega: function () { EditarPontoEntrega() },
            SalvarEdicaoPontoEntrega: function () { SalvarEdicaoPontoEntrega() },
            ExcluirDemanda: function () { ExcluirDemanda() },
            MontarModalOpcoes: function () { MontarBodyModalOpcoes() },
            verPosteStreet: function () { verPosteStreet() },
            EditarPoste: function () { EditarPoste() },
            DesativarLocalStreet: function () { DesativarLocalStreet() },
            NovoPoste: function () { NovoPoste() },
            NovoAnotacao: function () { NovoAnotacao() },
            ExcluirPoste: function () { ExcluirPoste() },
            SalvarNovaAnotacao: function () { SalvarNovaAnotacao() },
            CancelaAnotacao: function () { CancelaAnotacao() },
            MontarModalAnotacao: function () { MontarModalAnotacao() },
            ExcluirAnotacao: function () { ExcluirAnotacao() },
            SalvarEdicaoPoste: function () { SalvarEditPoste() },
            CancelarEdicaoPoste: function () { CancelarEdicaoPoste() },
            MontarModalStrands: function () { MontarModalStrands() },
            ApagarStrand: function () { ApagarStrand() },
            FechaModal: function () { FechaModal() },
            NovaDemanda: function () { NovaDemanda() },
            SalvarNovaDemanda: function () { SalvarNovaDemanda() },
            EditarAnotacao: function () { EditarAnotacao() },
            SalvarNovaAnotacaoEditado: function () { SalvarNovaAnotacaoEditado(idAnotacao) },
            NovoStrand: function () { NovoStrand() },
            CriarNovoStrand: function () { CriarNovoStrand() },
            CancelaNovaDemanda: function () { CancelaNovaDemanda() },
            CancelarEdicaoPontoEntrega: function () { CancelarEdicaoPontoEntrega() },
            SalvarArrastarPoste: function (idPosteArrastado) { SalvarArrastarPoste(idPosteArrastado) },
            CancelarArrastarPoste: function () { CancelarArrastarPoste() },
            RemoveFoto: function (identificador) { RemoveFoto(identificador); },
            AddFoto: function () { AddFoto(); },
            AddFotoPontoEntrega: function () { AddFotoPontoEntrega(); },
            RemoveFotoPontoEntrega: function (identificador) { RemoveFotoPontoEntrega(identificador); },
            NovaCidade: function () { NovaCidade(); },
            geolocate: function () { geolocate(); },
            CadastrarCidade: function () { CadastrarCidade(); },
            ProcurarRua: function () { ProcurarRua(); },
            LimparCampRua: function () { LimparCampRua();}
        };
    };
    return { get: function () { if (!instance) { instance = init(); } return instance; } };
})();

GlobalMaps.get().StartMap();