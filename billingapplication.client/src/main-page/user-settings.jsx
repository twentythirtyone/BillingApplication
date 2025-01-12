import { useState, useEffect } from "react";
import axios from "axios";

export const UserSettings = () => {
    const token = localStorage.getItem("token");
    const [userData, setUserData] = useState({
        email: "Нет данных",
    });
    const [isEmailEditing, setIsEmailEditing] = useState(false); // Состояние редактирования email
    const [isConfirming, setIsConfirming] = useState(false); // Состояние подтверждения email
    const [confirmationCode, setConfirmationCode] = useState("");
    const [newEmail, setnewEmail] = useState(""); 
    const [isPasswordEditing, setIsPasswordEditing] = useState(false); // Состояние изменения пароля
    const [oldPassword, setOldPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");

    const fetchUserData = async () => {
        try {
            const response = await axios.get("/billingapplication/subscribers/current", {
                headers: {
                    Authorization: `Bearer ${token}`,
                    Accept: "*/*",
                },
            });

            setUserData(response.data);
        } catch (error) {
            console.error("Ошибка при получении данных пользователя:", error);
        }
    };

    const changeEmailRequest = async () => {
        try {
            const response = await axios.post(
                "/billingapplication/subscribers/change/email",
                {}, // пустое тело
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        Accept: "*/*",
                    },
                }
            );

            alert(response.data);
            setIsConfirming(true); // Переход к этапу подтверждения
        } catch (error) {
            alert("Ошибка при отправке запроса: " + error);
        }
    };

    const confirmEmailChange = async () => {
        try {
            const response = await fetch(
                "/billingapplication/subscribers/change/email/confirm",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${token}`,
                        Accept: "*/*",
                    },
                    body: JSON.stringify({
                        newEmail: newEmail,
                        code: confirmationCode,
                    }),
                }
            );
    
            if (!response.ok) {
                const errorData = await response.json();
                alert("Ошибка при подтверждении кода: " + errorData.message || "Неизвестная ошибка");
                return;
            }
    
            alert("Email успешно изменен!");
            setIsEmailEditing(false);
            setIsConfirming(false);
            fetchUserData();
        } catch (error) {
            alert("Ошибка при отправке запроса: " + error.message);
        }
    };

    const handleEmailEdit = () => setIsEmailEditing(true);
    const handleCancelEdit = () => {
        setIsEmailEditing(false);
        setIsConfirming(false);
        setnewEmail("");
        setConfirmationCode("");
    };

    const changePasswordRequest = async () => {
        try {
            const response = await fetch(
                `/billingapplication/subscribers/change/password?lastPassword=${encodeURIComponent(oldPassword)}&password=${encodeURIComponent(newPassword)}`,
                {
                    method: "POST",
                    headers: {
                        Authorization: `Bearer ${token}`,
                        Accept: "*/*",
                    },
                    body: "",
                }
            );
    
            if (!response.ok) {
                const errorData = await response.json();
                alert("Ошибка при изменении пароля: " + errorData.message || "Неизвестная ошибка");
                return;
            }
    
            alert("Пароль успешно изменен!");
            setIsPasswordEditing(false);
            setOldPassword("");
            setNewPassword("");
        } catch (error) {
            alert("Ошибка при отправке запроса: " + error.message);
        }
    };

    const handlePasswordCancel = () => {
        setIsPasswordEditing(false);
        setOldPassword("");
        setNewPassword("");
    };

    useEffect(() => {
        document.title='Настройки';
        fetchUserData();
    }, []);

    return (
        <div className="user-settings">
            <h2>Аккаунт</h2>
            <div className="settings-sect">
                {!isEmailEditing ? (
                    <button className="settings-sub-tab" onClick={handleEmailEdit}>
                        <div>
                            <span>Email</span>
                            <p>{userData.email}</p>
                        </div>
                        <div className="settings-go">&#8250;</div>
                    </button>
                ) : isConfirming ? (
                    <div className="email-confirmation-container">
                        <div>
                            <p>Новый Email:</p>
                            <input className="email-confirmation-input"
                                type="text"
                                autoComplete="off"
                                value={newEmail}
                                onChange={(e) => setnewEmail(e.target.value)}
                            />
                        </div>
                        <div>
                            <p>Код подтверждения:</p>
                            <input className="email-confirmation-input"
                                type="text"
                                autoComplete="off"
                                value={confirmationCode}
                                onChange={(e) => setConfirmationCode(e.target.value)}
                            />
                        </div>
                        <div className="email-edit-actions">
                            <button onClick={confirmEmailChange}>Отправить</button>
                            <button onClick={handleCancelEdit}>Отмена</button>
                        </div>
                    </div>
                ) : (
                    <div className="email-edit-container">
                        <p>Хотите сменить Email?</p>
                        <div className="email-edit-actions">
                            <button onClick={changeEmailRequest}>Да</button>
                            <button onClick={handleCancelEdit}>Нет</button>
                        </div>
                    </div>
                )}
                {isPasswordEditing ? (
                    <div className="email-confirmation-container">
                        <div>
                            <p>Старый пароль:</p>
                            <input className="email-confirmation-input"
                                type="password"
                                autoComplete="off"
                                value={oldPassword}
                                onChange={(e) => setOldPassword(e.target.value)}
                            />
                        </div>
                        <div>
                            <p>Новый пароль:</p>
                            <input className="email-confirmation-input"
                                type="password"
                                autoComplete="off"
                                value={newPassword}
                                onChange={(e) => setNewPassword(e.target.value)}
                            />
                        </div>
                        <div className="email-edit-actions">
                            <button onClick={changePasswordRequest}>Подтвердить</button>
                            <button onClick={handlePasswordCancel}>Отмена</button>
                        </div>
                    </div>
                ) : (
                    <button
                        className="settings-sub-tab settings-sub-tab-password"
                        onClick={() => setIsPasswordEditing(true)}
                    >
                        <p>Пароль</p>
                        <div className="settings-go">&#8250;</div>
                    </button>
                )}
            </div>
            <div className="settings-sect-2">
                <button className="settings-sub-tab">
                    <div>
                        <span>Язык</span>
                        <p>Русский</p>
                    </div>
                    <div className="settings-go">&#8250;</div>
                </button>
                <button className="settings-sub-tab">
                    <div>
                        <span>Частые вопросы</span>
                        <p>Об операторе и услугах связи</p>
                    </div>
                    <div className="settings-go">&#8250;</div>
                </button>
            </div>
        </div>
    );
};