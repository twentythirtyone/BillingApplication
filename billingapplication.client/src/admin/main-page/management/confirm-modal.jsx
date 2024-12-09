const ConfirmModal = ({ message, onConfirm, onCancel }) => (
  <div className="modal">
    <p>{message}</p>
    <button onClick={onConfirm}>Да</button>
    <button onClick={onCancel}>Нет</button>
  </div>
);

export default ConfirmModal;