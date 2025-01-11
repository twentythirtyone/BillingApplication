import { useNavigate } from 'react-router-dom';

export const PleaseAuthorise = () => {
  const navigate = useNavigate();

  return (
    <div className="please-authorise">
      <div className="please-authorise__container">
        <h1 className="please-authorise__title">
          Доступ ограничен
        </h1>
        <p className="please-authorise__message">
          Кажется, вы не авторизованы или не имеете доступа к данной странице.
        </p>
        <button 
          className="please-authorise__button" 
          onClick={() => navigate('/login')}
        >
          Перейти на страницу авторизации
        </button>
      </div>
    </div>
  );
};