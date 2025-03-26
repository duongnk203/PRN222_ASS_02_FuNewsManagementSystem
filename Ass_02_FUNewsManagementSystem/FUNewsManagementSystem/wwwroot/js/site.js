$(() => {
    const params = new URLSearchParams(window.location.search);
    const newsArticleId = params.get("id"); // "123"

    // Khai báo loadComments trong phạm vi toàn cục
    function loadComments(newsArticleId) {
        $.ajax({
            url: "/api/Comments/GetComments",
            method: "GET",
            data: { newsArticleId: newsArticleId },
            success: (result) => {
                console.log("✅ Thành công! Dữ liệu nhận được:", result);
                let content = "";

                if (!result || result.length === 0) {
                    content = "<p class='text-muted'>Chưa có bình luận nào.</p>";
                } else {
                    $.each(result, (k, v) => {
                        let accountEmail = v.comment.account && v.comment.account.accountName
                            ? v.comment.account.accountName
                            : "Ẩn danh";
                        let formattedDate = v.comment.createdDate
                            ? new Date(v.comment.createdDate).toLocaleString()
                            : "Không xác định";

                        // Kiểm tra quyền chỉnh sửa & xóa
                        let editButton = v.canUpdate
                            ? `<button class="btn btn-sm btn-warning me-1" onclick="editComment(${v.comment.commentId})">✏ Sửa</button>`
                            : "";

                        let deleteButton = v.canDelete
                            ? `<button class="btn btn-sm btn-danger" onclick="deleteComment(${v.comment.commentId})">🗑 Xóa</button>`
                            : "";

                        content += `
                    <div class="comment-item p-3 border-bottom">
                        <div class="d-flex align-items-center">
                            <div class="avatar-circle bg-secondary text-white fw-bold me-2">
                                ${accountEmail.substring(0, 1).toUpperCase()}
                            </div>
                            <div class="flex-grow-1">
                                <strong class="text-dark">${accountEmail}</strong>
                                <small class="text-muted d-block">${formattedDate}</small>
                            </div>
                            <div>
                                ${editButton}
                                ${deleteButton}
                            </div>
                        </div>
                        <p class="mt-2 mb-1 text-dark" id="commentContent_${v.comment.commentId}">
                            ${v.comment.content || "Không có nội dung"}
                        </p>
                    </div>`;
                    });
                }

                $("#commentCount").html(result.length);
                $("#commentList").html(content);
            },
            error: function (xhr, status, error) {
                console.error("❌ Lỗi khi tải bình luận:", xhr.status, xhr.statusText, xhr.responseText);
            }
        });
    }



    // Kết nối SignalR
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();

    connection.start().then(() => {
        console.log("✅ SignalR đã kết nối.");
    }).catch(err => console.error("❌ Lỗi kết nối SignalR:", err));

    // Nhận thông báo cập nhật bình luận từ SignalR
    connection.on("ReceiveCommentUpdate", function () {
        console.log("📢 Nhận thông báo cập nhật bình luận!");
        loadComments(newsArticleId);
    });

    // Tải bình luận khi trang được load
    loadComments(newsArticleId);

});