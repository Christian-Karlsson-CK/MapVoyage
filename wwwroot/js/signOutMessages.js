var signOutMessages = [
    "Time to say goodbye. 😢",
    "Feeling adventurous? Let's sign out! 🌟",
    "Are you ready to log off and explore the real world? 🌍",
    "Till next time, adventurer! Sign out now? 🏕️",
    "Exit stage left? Sign out and take a bow! 🎭",
    "Embarking on a sign-out journey. Bon voyage! ⛵",
    "Leaving so soon? Farewell, explorer! 🚶‍♂️",
    "Signing off for now. Until our next digital encounter! 🖐️",
    "Breaking the virtual tether. Ready to fly? 🕊️",
    "It's been a blast, but time to hit the sign-out button! 💥"
];

$('#signOutModal').on('show.bs.modal', function () {
    var randomIndex = Math.floor(Math.random() * signOutMessages.length);
    var randomMessage = signOutMessages[randomIndex];
    $('#signOutModalLabel').text(randomMessage);
});
function showSignOutModal() {
    $('#signOutModal').modal('show');
}