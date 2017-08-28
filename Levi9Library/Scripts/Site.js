/*$(document).ready(function () {
	var table = $('#availableBooks').DataTable({
		"ajax": "data/arrays.txt",
		"columnDefs": [{
			"targets": -1,
			"data": null,
			"defaultContent": '@Html.ActionLink("Borrow", "Borrow", "Library")'
		}]
	});

	$('#availableBooks tbody').on('click', 'Html.ActionLink', function () {
		var data = table.row($(this).parents('tr')).data();
		alert(data[0] + "'s salary is: " + data[5]);
	});
});*/