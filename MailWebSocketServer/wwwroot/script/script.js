const ws = new WebSocket("ws://localhost:5047/ws");

function sendMail() {
    const from = document.getElementById("from").value.trim();
    const appPassword = document.getElementById("appPassword").value.trim();
    const to = document.getElementById("to").value.trim();
    const subject = document.getElementById("subject").value.trim();
    const body = document.getElementById("body").value.trim();

    if (!from || !appPassword || !to) {
        document.getElementById("response").innerText = "Vui lòng điền đầy đủ các trường!";
        return;
    }

    if (!to.includes("@") || !to.includes(".")) {
        document.getElementById("response").innerText = "Địa chỉ email người nhận không hợp lệ!";
        return;
    }
    

    const data = {
        fromEmail: from,
        appPassword: appPassword,
        to: to,
        subject: subject,
        body: body
    };
    

    // ✅ Log dữ liệu gửi lên để kiểm tra
    console.log("Sending:", data);

    ws.send("send:" + JSON.stringify(data));
}

ws.onmessage = function (event) {
    document.getElementById("response").innerText = event.data;
};

ws.onerror = function () {
    document.getElementById("response").innerText = "Không kết nối được WebSocket!";
};
