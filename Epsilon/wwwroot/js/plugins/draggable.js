(function ($) {
    $.fn.dragablito = function (opt) {

        opt = $.extend({ handle: "", cursor: "move" }, opt);
        var main_el = this;
        var $el = null;
        if (opt.handle === "") {
            $el = this;
        } else {
            $el = this.find(opt.handle);
        }

        let mouseDownEvent = (e) => {
            var $drag = main_el.addClass('draggable');

            var drg_h = $drag.outerHeight(),
                drg_w = $drag.outerWidth(),
                pos_y = $drag.offset().top + drg_h - e.pageY,
                pos_x = $drag.offset().left + drg_w - e.pageX;

            let mouseMoveEvent = (e) => {

                $('.draggable').offset({
                    top: e.pageY + pos_y - drg_h,
                    left: e.pageX + pos_x - drg_w
                });
            }

            let mouseUpTitleEvent = (e) => {
                $(this).removeClass('draggable');
            }

            $drag.parents()
                .off("mousemove", mouseMoveEvent).on("mousemove", mouseMoveEvent)
                .off("mouseup", mouseUpTitleEvent).on("mouseup", mouseUpTitleEvent);

            e.preventDefault(); // disable selection
        }

        let mouseUpEvent = (e) => {
            main_el.removeClass('draggable');
            e.preventDefault(); // disable selection
        }

        $el.css('cursor', opt.cursor);
        $el.off("mousedown", mouseDownEvent).on("mousedown", mouseDownEvent);
        $el.off("mouseup", mouseUpEvent).on("mouseup", mouseUpEvent);

        return this;
    }
}(jQuery));



