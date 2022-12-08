// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


var login = document.getElementById("container");
var menu = document.getElementById("menu");
menu.style.display = "none";

var email = document.querySelector(".email");
var password = document.querySelector(".matkhau");

var btn_dangnhap = document.querySelector(".btn_dangnhap");


z = function () {

    if (email.value == "admin" && password.value == "admin") {
        login.style.display = "none";
        menu.style.display = "block"
        console.log(true)
    } else {
        alert("Sai tài khoảng mật khẩu rồi")
    }

}
btn_dangnhap.addEventListener('click', z);
