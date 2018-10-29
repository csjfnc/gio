$(document).ready(function () {
    $('#example').DataTable();
});
function ExcluirUser(idUser) {
    bootbox.confirm("<span class='glyphicon glyphicon-trash'></span> Deseja realmente excluir esse Usuario ?", function (result) {
        if (result) {
			window.location.href = "/Usuario/ExcluirDefinitivo?id_user=" + idUser;
		}
        else { /*Nada acontece Ele nao Confirmou!*/ }
    });
}
