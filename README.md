# Buoi07_TinhToan3

## Mô tả

**Buoi07_TinhToan3** là chương trình **máy tính đơn giản** hỗ trợ các
phép toán cơ bản trên số thực (cộng, trừ, nhân, chia). Mục tiêu: giao
diện thân thiện, kiểm tra đầu vào chặt chẽ (hạn chế ký tự lạ, không để
trống, giới hạn độ dài), xử lý chính xác số rất lớn/nhỏ bằng kiểu số có
độ chính xác cao, và đưa ra thông báo lỗi rõ ràng theo đặc tả.

------------------------------------------------------------------------

## Tính năng chính

-   Hỗ trợ số thực dương và âm (cả phần nguyên và phần thập phân).
-   Hỗ trợ tính toán với **tối đa 30 chữ số** (tổng số chữ số trong phần
    nguyên + phần thập phân).
-   Kiểm tra nhập liệu tức thời (khi mất focus) và không cho phép tiếp
    tục nếu dữ liệu không hợp lệ.
-   Khi một ô nhập nhận focus, toàn bộ nội dung trong ô được **tự động
    chọn** (select all).
-   Người dùng chỉ được chọn **một phép toán** tại một thời điểm (radio
    button).
-   Kết quả hiển thị trong ô **chỉ đọc** (read-only).
-   Xử lý đặc biệt phép chia cho 0: hiển thị lỗi và chuyển focus về ô số
    chia.
-   Nút "Thoát" cần hộp thoại xác nhận trước khi đóng chương trình.

------------------------------------------------------------------------

## Yêu cầu chức năng

1.  **Phạm vi nhập:** nhận các số thực, gồm dấu +/-, phần nguyên và phần
    thập phân.
2.  **Giới hạn độ dài:** mỗi số không được vượt quá **30 chữ số** (đếm
    chữ số 0--9 tổng hợp giữa phần nguyên & thập phân). *Không tính dấu
    (+/-) và dấu chấm thập phân vào số chữ số.*
3.  **Bắt buộc:** hai ô số (Số 1, Số 2) **không được để trống**. Nếu để
    trống → khi ô mất focus hiển thị lỗi, yêu cầu sửa và **không cho
    phép làm thao tác khác** trước khi sửa.
4.  **Kiểm tra ký tự:** không cho phép ký tự khác ngoài `0-9`, dấu
    `+`/`-`, và dấu thập phân (`.` hoặc `,` --- xem phần "Chuẩn hóa
    dấu"). Nếu phát hiện ký tự lạ → báo lỗi ngay khi ô mất focus.
5.  **Hành vi focus:** khi ô nhập được focus, toàn bộ nội dung phải được
    chọn (select all).
6.  **Phép toán:** chỉ chọn 1 phép toán tại một thời điểm (UI: radio
    buttons).
7.  **Tính toán & kết quả:** khi đã nhập hợp lệ cả hai số và chọn phép
    toán, nhấn nút *Tính* → hiển thị kết quả ở ô kết quả (readonly).
8.  **Chia cho 0:** nếu phép toán là chia và Số 2 = 0 → hiển thị thông
    báo lỗi, đặt focus vào Số 2, yêu cầu nhập lại.
9.  **Thoát:** nhấn *Thoát* → hiển thị hộp thoại xác nhận ("Bạn có chắc
    muốn thoát?"). Nếu Đồng ý → đóng; nếu Hủy → trở lại chương trình.

------------------------------------------------------------------------

## Các điểm cải thiện trong bài gồm
- Kiểm tra phép chia với 0
- Kiểm tra nhập vào những ký tự lạ không phải số ở ô thứ nhất
- Kiểm tra nhập vào những ký tự lạ không phải số ở ô thứ hai
- Kiểm tra để trống ô thứ nhất
- Kiểm tra để trống ô thứ hai
- Kiểm tra độ dài vượt quá 30 số ở ô thứ nhất
- Kiểm tra độ dài vượt quá 30 số ở ô thứ hai
- Kiểm tra chính xác những phép tính với số cực lớn
- Kiểm tra chính xác những phép tính với số cực nhỏ
- Thực hiện trình tự các bước và thông báo như đặc tả yêu cầu
