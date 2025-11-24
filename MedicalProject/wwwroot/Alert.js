// Alert.js - نسخه اصلاح شده بدون Razor
function showAlert(icon, title, text, timer = null) {
    const config = {
        icon: icon,
        title: title,
        text: text,
        confirmButtonText: 'متوجه شدم',
        timerProgressBar: true,
        customClass: {
            popup: 'persian-swal',
            title: 'persian-swal-title',
            htmlContainer: 'persian-swal-text'
        }
    };

    if (timer) {
        config.timer = timer;
        config.showConfirmButton = false;
    }

    return Swal.fire(config);
}

// آبجکت اصلی برای همه پیام‌ها
const Alert = {
    // ✅ موفقیت‌ها
    success: (message, title = 'موفقیت') => {
        return showAlert('success', title, message, 3000);
    },

    // ❌ خطاهای عمومی
    error: (message, title = 'خطا') => {
        return showAlert('error', title, message, 5000);
    },

    // 🔍 خطای 404 - Not Found
    notFound: (message = 'مورد درخواستی یافت نشد') => {
        return showAlert('error', 'یافت نشد (404)', message, 5000);
    },

    // 🚨 خطای 500 - Server Error
    serverError: (message = 'خطای داخلی سرور') => {
        return showAlert('error', 'خطای سرور (500)', message, 6000);
    },

    // ⚠️ خطای 400 - Bad Request
    badRequest: (message = 'درخواست نامعتبر') => {
        return showAlert('error', 'درخواست نامعتبر (400)', message, 5000);
    },

    // 🔒 خطای 401 - Unauthorized
    unauthorized: (message = 'احراز هویت لازم است') => {
        return showAlert('error', 'دسترسی غیرمجاز (401)', message, 5000);
    },

    // 🚫 خطای 403 - Forbidden
    forbidden: (message = 'شما دسترسی لازم را ندارید') => {
        return showAlert('error', 'ممنوع (403)', message, 5000);
    },

    // ⏰ خطای 408 - Timeout
    timeout: (message = 'زمان درخواست به پایان رسید') => {
        return showAlert('error', 'timeout (408)', message, 5000);
    },

    // 💾 خطای 409 - Conflict
    conflict: (message = 'تضاد در داده‌ها') => {
        return showAlert('error', 'تضاد (409)', message, 5000);
    },

    // 📦 خطای 422 - Validation Error
    validationError: (message = 'خطای اعتبارسنجی داده‌ها') => {
        return showAlert('error', 'خطای اعتبارسنجی (422)', message, 5000);
    },

    // ⚠️ هشدارها
    warning: (message, title = 'هشدار') => {
        return showAlert('warning', title, message, 4000);
    },

    // 📝 هشدار اعتبارسنجی
    validationWarning: (message = 'لطفاً اطلاعات را بررسی کنید') => {
        return showAlert('warning', 'خطای اعتبارسنجی', message, 5000);
    },

    // 💰 هشدار مالی
    financialWarning: (message = 'موجودی کافی نیست') => {
        return showAlert('warning', 'هشدار مالی', message, 5000);
    },

    // 🔐 هشدار امنیتی
    securityWarning: (message = 'عملیات غیرامن تشخیص داده شد') => {
        return showAlert('warning', 'هشدار امنیتی', message, 6000);
    },

    // ℹ️ اطلاعیه‌ها
    info: (message, title = 'اطلاعیه') => {
        return showAlert('info', title, message, 4000);
    },

    // 🏢 اطلاعیه سیستم
    systemInfo: (message = 'سیستم در حال بروزرسانی است') => {
        return showAlert('info', 'اطلاعیه سیستم', message, 4000);
    },

    // 📊 اطلاعیه مالی
    financialInfo: (message = 'تراکنش با موفقیت انجام شد') => {
        return showAlert('info', 'اطلاعیه مالی', message, 4000);
    },

    // 🔄 تاییدیه (Confirm)
    confirm: (title, text, confirmText = 'بله، مطمئنم', cancelText = 'خیر') => {
        return Swal.fire({
            title: title,
            text: text,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: confirmText,
            cancelButtonText: cancelText,
            customClass: {
                popup: 'persian-swal'
            }
        });
    },

    // 🔄 تاییدیه حذف
    confirmDelete: (itemName = 'این آیتم') => {
        return Alert.confirm(
            'تایید حذف',
            `آیا از حذف ${itemName} مطمئن هستید؟ این عمل غیرقابل بازگشت است.`,
            'بله، حذف کن',
            'لغو'
        );
    },

    // 🔄 تاییدیه تراکنش مالی
    confirmTransaction: (amount, description = '') => {
        return Alert.confirm(
            'تایید تراکنش',
            `آیا از انجام تراکنش به مبلغ ${amount} تومان ${description} مطمئن هستید؟`,
            'بله، پرداخت کن',
            'لغو'
        );
    },

    // ⏳ لودینگ
    loading: (title = 'لطفاً منتظر بمانید...') => {
        Swal.fire({
            title: title,
            allowEscapeKey: false,
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            },
            customClass: {
                popup: 'persian-swal'
            }
        });
    },

    // ❌ بستن
    close: () => {
        Swal.close();
    }
};

// تابع decode برای کاراکترهای فارسی
function decodePersianText(encodedText) {
    if (!encodedText) return '';
    const textarea = document.createElement('textarea');
    textarea.innerHTML = encodedText;
    return textarea.value;
}

// نمایش خودکار پیام‌های TempData - نسخه جدید
function showTempDataMessages() {
    // موفقیت
    const successMessage = document.getElementById('tempdata-success')?.value;
    if (successMessage) {
        Alert.success(decodePersianText(successMessage));
    }

    // خطا
    const errorMessage = document.getElementById('tempdata-error')?.value;
    if (errorMessage) {
        const decodedError = decodePersianText(errorMessage);
        if (decodedError.includes('404') || decodedError.includes('یافت نشد')) {
            Alert.notFound(decodedError);
        } else if (decodedError.includes('500') || decodedError.includes('سرور')) {
            Alert.serverError(decodedError);
        } else if (decodedError.includes('400') || decodedError.includes('نامعتبر')) {
            Alert.badRequest(decodedError);
        } else if (decodedError.includes('401') || decodedError.includes('دسترسی')) {
            Alert.unauthorized(decodedError);
        } else {
            Alert.error(decodedError);
        }
    }

    // هشدار
    const warningMessage = document.getElementById('tempdata-warning')?.value;
    if (warningMessage) {
        const decodedWarning = decodePersianText(warningMessage);
        if (decodedWarning.includes('اعتبارسنجی') || decodedWarning.includes('بررسی')) {
            Alert.validationWarning(decodedWarning);
        } else if (decodedWarning.includes('مالی') || decodedWarning.includes('موجودی')) {
            Alert.financialWarning(decodedWarning);
        } else if (decodedWarning.includes('امنیتی') || decodedWarning.includes('غیرامن')) {
            Alert.securityWarning(decodedWarning);
        } else {
            Alert.warning(decodedWarning);
        }
    }

    // اطلاعیه
    const infoMessage = document.getElementById('tempdata-info')?.value;
    if (infoMessage) {
        const decodedInfo = decodePersianText(infoMessage);
        if (decodedInfo.includes('سیستم') || decodedInfo.includes('بروزرسانی')) {
            Alert.systemInfo(decodedInfo);
        } else if (decodedInfo.includes('مالی') || decodedInfo.includes('تراکنش')) {
            Alert.financialInfo(decodedInfo);
        } else {
            Alert.info(decodedInfo);
        }
    }
}

// استایل‌های سفارشی برای فارسی
function addPersianStyles() {
    if (document.querySelector('.persian-swal-styles')) return;

    const style = document.createElement('style');
    style.className = 'persian-swal-styles';
    style.textContent = `
        .persian-swal {
            font-family: "Tahoma", "Segoe UI", sans-serif !important;
            direction: rtl !important;
            text-align: right !important;
        }
        .persian-swal-title {
            font-size: 18px !important;
            font-weight: bold !important;
            margin-bottom: 15px !important;
        }
        .persian-swal-text {
            font-size: 14px !important;
            line-height: 1.6 !important;
        }
        .swal2-confirm, .swal2-cancel {
            font-family: "Tahoma", "Segoe UI", sans-serif !important;
            font-size: 14px !important;
        }
    `;
    document.head.appendChild(style);
}

// هندلر خطاهای AJAX/Fetch
function setupAjaxErrorHandling() {
    // هندلر خطا برای Fetch API
    const originalFetch = window.fetch;
    window.fetch = async function (...args) {
        try {
            const response = await originalFetch.apply(this, args);
            if (!response.ok) {
                handleHttpError(response.status, response.statusText);
            }
            return response;
        } catch (error) {
            Alert.error('خطا در ارتباط با سرور');
            throw error;
        }
    };

    // هندلر خطا برای jQuery AJAX
    if (window.jQuery) {
        $(document).ajaxError(function (event, jqXHR, ajaxSettings, thrownError) {
            handleHttpError(jqXHR.status, thrownError);
        });
    }
}

// هندلر خطاهای HTTP
function handleHttpError(status, message) {
    switch (status) {
        case 400:
            Alert.badRequest(message);
            break;
        case 401:
            Alert.unauthorized(message);
            break;
        case 403:
            Alert.forbidden(message);
            break;
        case 404:
            Alert.notFound(message);
            break;
        case 408:
            Alert.timeout(message);
            break;
        case 409:
            Alert.conflict(message);
            break;
        case 422:
            Alert.validationError(message);
            break;
        case 500:
            Alert.serverError(message);
            break;
        default:
            Alert.error(`خطای ${status}: ${message}`);
    }
}

// هندلر خطاهای全局 JavaScript
function setupGlobalErrorHandling() {
    window.addEventListener('error', function (event) {
        console.error('Global error:', event.error);
        Alert.error('خطای غیرمنتظره در سیستم رخ داد');
    });

    window.addEventListener('unhandledrejection', function (event) {
        console.error('Unhandled promise rejection:', event.reason);
        Alert.error('خطا در پردازش درخواست');
    });
}

// مقداردهی اولیه
function initializeAlertSystem() {
    addPersianStyles();
    showTempDataMessages();
    setupAjaxErrorHandling();
    setupGlobalErrorHandling();
}

// قرار دادن در global scope
window.Alert = Alert;
window.handleHttpError = handleHttpError;
window.initializeAlertSystem = initializeAlertSystem;

// اتوماتیک initialize کردن وقتی DOM لود شد
document.addEventListener('DOMContentLoaded', function () {
    initializeAlertSystem();
});

// نمایش خودکار پیام‌های TempData
//function showTempDataMessages() {
//    // موفقیت
//    @if (TempData["Success"] != null) {
//        <text>
//            Alert.success(decodePersianText('@TempData["Success"]'));
//        </text>
//    }

//    // خطا
//    @if (TempData["Error"] != null) {
//        <text>
//            const errorMsg = decodePersianText('@TempData["Error"]');
//            if (errorMsg.includes('404') || errorMsg.includes('یافت نشد')) {
//                Alert.notFound(errorMsg);
//                    } else if (errorMsg.includes('500') || errorMsg.includes('سرور')) {
//                Alert.serverError(errorMsg);
//                    } else if (errorMsg.includes('400') || errorMsg.includes('نامعتبر')) {
//                Alert.badRequest(errorMsg);
//                    } else if (errorMsg.includes('401') || errorMsg.includes('دسترسی')) {
//                Alert.unauthorized(errorMsg);
//                    } else {
//                Alert.error(errorMsg);
//                    }
//        </text>
//    }

//    // هشدار
//    @if (TempData["Warning"] != null) {
//        <text>
//            const warningMsg = decodePersianText('@TempData["Warning"]');
//            if (warningMsg.includes('اعتبارسنجی') || warningMsg.includes('بررسی')) {
//                Alert.validationWarning(warningMsg);
//                    } else if (warningMsg.includes('مالی') || warningMsg.includes('موجودی')) {
//                Alert.financialWarning(warningMsg);
//                    } else if (warningMsg.includes('امنیتی') || warningMsg.includes('غیرامن')) {
//                Alert.securityWarning(warningMsg);
//                    } else {
//                Alert.warning(warningMsg);
//                    }
//        </text>
//    }

//    // اطلاعیه
//    @if (TempData["Info"] != null) {
//        <text>
//            const infoMsg = decodePersianText('@TempData["Info"]');
//            if (infoMsg.includes('سیستم') || infoMsg.includes('بروزرسانی')) {
//                Alert.systemInfo(infoMsg);
//                    } else if (infoMsg.includes('مالی') || infoMsg.includes('تراکنش')) {
//                Alert.financialInfo(infoMsg);
//                    } else {
//                Alert.info(infoMsg);
//                    }
//        </text>
//    }
//}
