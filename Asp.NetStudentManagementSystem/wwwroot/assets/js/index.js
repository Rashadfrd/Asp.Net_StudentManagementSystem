let sidebarToggle = document.querySelector(".sidebar-menu-toggle")
let sideBarTitle = document.querySelectorAll(".sidebar-menu-title")
let sideBar = document.querySelector(".aside")
let switchToggle = document.querySelector("#switch") 
let profileCard = document.querySelector(".profile-card")
let sidebarAnchor = document.querySelectorAll(".sidebar-menu__item a")
let table = document.querySelector(".subjects-table")
let uniInfoCard = document.querySelectorAll(".uniInfo-card")
let modalContent = document.querySelector(".modal-content")
let mainHeader = document.querySelector(".main-content-header")

let count = 0;
sidebarToggle.addEventListener("click",function () {
    if (count != 0) {
        sideBarTitle.forEach(val => {
            val.classList.remove("toggle")
            sideBar.style.width = "15vw"
            count++
        });
    }
    else{
        sideBarTitle.forEach(val => {
            val.classList.add("toggle")
            sideBar.style.width = "4vw"
            count--
        });
    }
})


function darkmode(){
    switchToggle.checked = true;
    if(table){
        table.classList.remove("table-striped")
        table.classList.add("dark-mode-item")
    } 
    if(uniInfoCard){
        uniInfoCard.forEach(element => {
            element.classList.add("dark-mode-body")
            element.classList.add("dark-mode-item")
        });
    }
    if (modalContent) {
        modalContent.classList.add("dark-mode-body")
    }
    document.body.classList.add("dark-mode-body")
    profileCard.classList.add("dark-mode-item")
    mainHeader.classList.add("dark-mode-item")
    mainHeader.classList.add("dark-mode-body")
    sidebarAnchor.forEach(element => {
        element.classList.add("dark-mode-item")
    });
    localStorage.setItem("mode", "dark");
    }



function nodark(){
    switchToggle.checked = false;
    if(table){
        table.classList.remove("dark-mode-item")
        table.classList.add("table-striped")
    }
    if(uniInfoCard){
        uniInfoCard.forEach(element => {
            element.classList.remove("dark-mode-body")
            element.classList.remove("dark-mode-item")
        });
    }
    if (modalContent) {
        modalContent.classList.remove("dark-mode-body")
    }
    mainHeader.classList.remove("dark-mode-item")
    mainHeader.classList.remove("dark-mode-body")
    document.body.classList.remove("dark-mode-body")
    if (profileCard) {

        profileCard.classList.remove("dark-mode-item")
    }
        sidebarAnchor.forEach(element => {
            element.classList.remove("dark-mode-item")
        });
        localStorage.setItem("mode", "light");
    }

  if(localStorage.getItem("mode")=="dark")
        darkmode();
  else
    nodark();


switchToggle.addEventListener("change",function () {
    if (this.checked == true) {
        darkmode()
    } else {
        nodark()
    }
})