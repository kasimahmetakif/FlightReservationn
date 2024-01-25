document.querySelectorAll(".aircraft .seat").forEach(function (seat) {
  seat.addEventListener("click", function () {
    this.classList.toggle("selected");
  });
});
