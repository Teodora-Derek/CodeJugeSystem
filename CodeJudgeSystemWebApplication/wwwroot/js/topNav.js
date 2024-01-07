const setActiveNavElem = () => {
    const navItems = document.querySelectorAll("#topnav a");

    const navIndex = parseInt(localStorage.getItem("navIndex")) || 0;
    navItems.forEach((item, i) => {
        if (i === navIndex) item.classList.add("active");
    });

    console.log(navItems);
    navItems.forEach((item, index) => {
        item.addEventListener("click", () => {
            localStorage.setItem("navIndex", index);
            navItems.forEach((item) => {
                item.classList.remove("active");
            });
            item.classList.add("active");
        });
    });
};
setActiveNavElem();