const menuIcon = document.getElementById("menu-icon");
let sideItems = document.querySelectorAll(".navbar-left-side-item");
let sideLabels = document.querySelectorAll(".side-label");
let sideIcons = document.querySelectorAll(".side-icon");


let catCollapseBtn = document.getElementById("catCollapseBtn");
let catChevronRight = document.getElementById("catChevronRight");
let itemCollapseBtn = document.getElementById("itemCollapseBtn");
let itemChevronRight = document.getElementById("itemChevronRight");
let mainSidebar = document.getElementById("mainSidebar");
let mainSidebarItem = document.getElementById("mainSidebarItem");
let mainContent = document.getElementById("mainContent");

catCollapseBtn.onclick = function () {    
    catChevronRight.classList.toggle("rotate");
}

itemCollapseBtn.onclick = function () {
    itemChevronRight.classList.toggle("rotate");
}

menuIcon.onclick = function minimize() {
    mainSidebar.classList.toggle("transition-effect1");
    mainSidebar.classList.toggle("transition-effect2");
    mainSidebarItem.classList.toggle("transition-effect1");
    mainSidebarItem.classList.toggle("transition-effect2");
    mainContent.classList.toggle("transition-effect1");
    mainContent.classList.toggle("transition-effect2");
    mainSidebar.classList.toggle("col-xl-2");
    mainSidebar.classList.toggle("col-xl-1");
    mainSidebar.classList.toggle("col-2");
    mainSidebar.classList.toggle("col-1");
    mainSidebar.classList.toggle("col-sm-3");
    mainSidebar.classList.toggle("col-sm-1");
    mainContent.classList.toggle("col-xl-10");
    mainContent.classList.toggle("col-xl-11");
    mainContent.classList.toggle("col-10");
    mainContent.classList.toggle("col-11");
    mainContent.classList.toggle("col-sm-9");
    mainContent.classList.toggle("col-sm-11");
    sideLabels.forEach(label => {
        label.classList.toggle('d-md-none')       
    });
    sideIcons.forEach(icon => {
        icon.classList.toggle("text-center");
    })
    mainSidebar.classList.toggle("hamburger-click");
};

