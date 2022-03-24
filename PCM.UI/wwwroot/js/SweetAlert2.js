Swal.fire({
	// title:
	// text:
	// html:
	// icon:
	// confirmButtonText:
	// footer:
	// width:
	// padding:
	// background:
	// grow:
	// backdrop:
	// timer:
	// timerProgressBar:
	// toast:
	// position:
	// allowOutsideClick:
	// allowEscapeKey:
	// allowEnterKey:
	// stopKeydownPropagation:

	// input:
	// inputPlaceholder:
	// inputValue:
	// inputOptions:
	
	//  customClass:
	// 	container:
	// 	popup:
	// 	header:
	// 	title:
	// 	closeButton:
	// 	icon:
	// 	image:
	// 	content:
	// 	input:
	// 	actions:
	// 	confirmButton:
	// 	cancelButton:
	// 	footer:	

	// showConfirmButton:
	// confirmButtonColor:
	// confirmButtonAriaLabel:

	// showCancelButton:
	// cancelButtonText:
	// cancelButtonColor:
	// cancelButtonAriaLabel:
	
	// buttonsStyling:
	// showCloseButton:
	// closeButtonAriaLabel:


	// imageUrl:
	// imageWidth:
	// imageHeight:
	// imageAlt:
});
$(document).ready(function () {
	$("#deleteMedicine").on('click', function () {
		Swal.fire({
			title: "Esta Seguro",
			// text:
			html: '<button type="submit" class="btn btn-danger btn-sm" asp-page-handler="Delete" asp-route-Id="@medicine.Id"><img alt="delete" src="~/Icons/delete-white-18dp.svg" /></button>',
			icon: 'wanring',
			// confirmButtonText:
			// footer:
			// width:
			// padding:
			// background:
			// grow:
			// backdrop:
			// timer:
			// timerProgressBar:
			// toast:
			// position:
			// allowOutsideClick:
			// allowEscapeKey:
			// allowEnterKey:
			// stopKeydownPropagation:

			// input:
			// inputPlaceholder:
			// inputValue:
			// inputOptions:

			//  customClass:
			// 	container:
			// 	popup:
			// 	header:
			// 	title:
			// 	closeButton:
			// 	icon:
			// 	image:
			// 	content:
			// 	input:
			// 	actions:
			// 	confirmButton:
			// 	cancelButton:
			// 	footer:	

			// showConfirmButton:
			// confirmButtonColor:
			// confirmButtonAriaLabel:

			showCancelButton: true,
			// cancelButtonText:
			// cancelButtonColor:
			 cancelButtonAriaLabel: "Cancelar"

			// buttonsStyling:
			// showCloseButton:
			// closeButtonAriaLabel:


			// imageUrl:
			// imageWidth:
			// imageHeight:
			// imageAlt:
		});
	});
});