var nGroups = 1;

$(document).ready(function () {
	$(".group").change(function () {
		itChanged();
	});
});

function itChanged() {
	var total = 0;
	var obj = $('.group');
	var ok = true;

	for (var i = 0; i < obj.length; i++) {
		if (parseInt(obj[i].value))
			total += parseInt(obj[i].value);
		else {
			alert('Please enter a number');
			ok = false;
			break;
		}
	}

	if (ok) {
		console.log(total);
		if (total > totalPersons) {
			$('#p-toomuch').show();
			$('#p-remaining').hide();
			$('#p-good').hide();
			$('.number').html(total - totalPersons);
		}
		else if (total < totalPersons) {
			$('#p-toomuch').hide();
			$('#p-remaining').show();
			$('#p-good').hide();
			$('.number').html(totalPersons - total);
		}
		else {
			$('#p-toomuch').hide();
			$('#p-remaining').hide();
			$('#p-good').show();
			$('input[type="submit"]').removeAttr('disabled');
		}
	}
	else {
		
	}

}

//little problem... but works
$('#add-group').click(function () {
	++nGroups;
	$('#groups').append('<li><input type="text" class="group" value = "1" /></li>');
	$(".group").die('change');
	$(".group").change(function () {
		itChanged();
	});
});