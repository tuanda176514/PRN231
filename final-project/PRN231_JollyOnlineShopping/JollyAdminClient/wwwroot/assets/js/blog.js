


const notifyRemoveBlog = (id) => {
    swalConfirm("Bạn có chắc muốn xóa bài viết này?").then((result) => {
        if (result.isConfirmed) {
            try {
                fetch(`https://localhost:8888/blogs/remove/${id}`, {
                    method: "DELETE",
                })
                    .then((response) => {
                        if (response.ok) {
                            swalSuccess("Bài viết đã được xóa thành công!").then(() => {
                                window.location.href = "/blog";
                            });
                        } else {
                            return response.json();
                        }
                    })
                    .then((errorData) => {
                        if (errorData) {
                            swalFailed(`Xóa bài viết thất bại! ${errorData.Message}`);
                        }
                    })
                    .catch((error) => {
                        console.error("Error:", error);
                    });
            } catch (e) {
                console.error("Error:", e);
            }
        }
    });
};

const notifyCreateBlog = () => {
    try {
        fetch(`https://localhost:8888/blogs/create`, {
            method: "POST",
            body: new FormData(document.getElementById("blog_form")),
        })
            .then((response) => {
                if (response.ok) {
                    swalSuccess("Bài viết đã được thêm thành công!").then(() => {
                        window.location.href = "/blog";
                    });
                } else {
                    return response.json();
                }
            })
            .then((errorData) => {
                if (errorData) {
                    swalFailed(`Thêm bài viết thất bại! ${errorData.Message}`);
                }
            })
            .catch((error) => {
                console.error("Error:", error);
            });
    } catch (e) {
        console.error("Error:", e);
    }
};

const notifyUpdateBlog = (id) => {
    const data = {
        id: document.getElementById("id").value,
        title: document.getElementById("title").value,
        shortContent: document.getElementById("shortContent").value,
        content: document.getElementById("content").value,
        publishedDate: document.getElementById("publishedDate").value,
        image: document.getElementById("updateimage").value,
        userId: document.getElementById("userId").value
    }

    try {
        fetch(`https://localhost:8888/blogs/edit/${id}`, {
            method: "PUT",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data),
        })
            .then((response) => {
                if (response.ok) {
                    swalSuccess("Bài viết đã được sửa thành công!").then(() => {
                        window.location.href = "/blog";
                    });
                } else {
                    return response.json();
                }
            })
            .then((errorData) => {
                if (errorData) {
                    swalFailed(`Sửa bài viết thất bại! ${errorData.Message}`);
                }
            })
            .catch((error) => {
                console.error("Error:", error);
            });
    } catch (e) {
        console.error("Error:", e);
    }
};
