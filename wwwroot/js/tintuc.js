import nav from '../component/nav.js'
const header_img = document.querySelector('.header');
$(document).ready(function(){
    $("#nav").html(nav.giaodien );
    $(".nav__mobile-icon").click(nav.onpen)
    $("footer").html(nav.footer);
    nav.checkin(2,4);
    
})