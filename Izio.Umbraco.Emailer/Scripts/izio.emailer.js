$(function () {

    var label = $("<label></label>").attr({ "for": "confirmation" }).text("Confirm you are not a spam bot");
    var input = $("<input />").attr({ type: "checkbox", id: "confirmation", name: "confirmation", value: "true" });
    var item = $("<li></li>").insertBefore($("#contact-form li").last());

    item.append(label).append(input);

    $("#contact-form").verify({
        prompt: function (element, text) {

            if (text === "" || text == null) {
                $("label[for='" + element[0].id + "'].error").remove();
            } else {
                $("label[for='" + element[0].id + "'].error").remove();

                if ($("label[for='" + element[0].id + "'].error:contains(" + text + ")").length === 0) {
                    $("<label for='" + element[0].id + "' class='error'>" + text + "</label>").insertAfter(element);
                }
            }
        }
    });

    if ($(".validation-summary-errors").length) {
        $("html, body").animate({
            scrollTop: $(".validation-summary-errors").offset().top
        }, 1000);
    }

    if ($(".confirmation").length) {
        $("html, body").animate({
            scrollTop: $(".confirmation").offset().top
        }, 1000);
    }

});