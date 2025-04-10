const ws = new WebSocket("ws://localhost:5047/ws");

function sendMail() {
    const from = document.getElementById("from").value;
    const appPassword = document.getElementById("appPassword").value;
    const to = document.getElementById("to").value;
    const subject = document.getElementById("subject").value;
    const body = document.getElementById("body").value;

    if (!from || !appPassword || !to) {
        document.getElementById("response").innerText = "Vui lòng điền đầy đủ các trường!";
        return;
    }

    const data = { fromEmail: from, appPassword, to, subject, body };
    ws.send("send:" + JSON.stringify(data));
}

ws.onmessage = function (event) {
    document.getElementById("response").innerText = event.data;
};

ws.onerror = function () {
    document.getElementById("response").innerText = "Không kết nối được WebSocket!";
};
