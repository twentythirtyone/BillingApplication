const ConfirmModal = ({ message, onConfirm, onCancel }) => (
  <div className="tariff-confirm-modal">
    <p>{message}</p>
    <div className="tariff-modal-buttons tariff-confirm">
        <button onClick={onConfirm}>Да</button>
        <button onClick={onCancel}>Нет</button>
    </div>
  </div>
);

export default ConfirmModal;