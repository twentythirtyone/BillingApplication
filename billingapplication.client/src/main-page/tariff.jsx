import { AdditionalServices } from './additional-services.jsx';
import ReactLoading from 'react-loading';
import { useEffect, useState } from 'react';
import { getRandomObjects, timeToMinutes } from './functions.js';
import { getTariff, changeTariff, payTariff } from '../requests.jsx';

const Tariff = () => {
    const [tariffs, setTariffs] = useState([]);
    const [isProcessing, setIsProcessing] = useState(false);

    useEffect(() => {
        const token = localStorage.getItem('token');
        const fetchTariff = async (token) => {
            const tarif = await getTariff(token);
            setTariffs(tarif);
        };
        if (token) {
            fetchTariff(token);
        }
    }, []);

    const handleTariffChange = async (tariff) => {
        const userConfirmed = window.confirm(
            `Вы действительно хотите сменить тариф на "${tariff.title}"? 
Учтите, что с вашего счета будет списана плата ${tariff.price}₽.`
        );

        if (userConfirmed) {
            try {
                setIsProcessing(true);
                await changeTariff(tariff.id);
                await payTariff(tariff.id);
                alert('Тариф успешно изменен и оплачен!');
            } catch (error) {
                alert(error.message);
            } finally {
                setIsProcessing(false);
            }
        }
    };

    const slicedTariffsArray = getRandomObjects(tariffs, 3);
    const mainTariff = getRandomObjects(tariffs, 1)[0];

    if (!tariffs.length) {
        return <ReactLoading type="cylon" color="#FF3B30" height={667} width={375} className='loading'/>;
    }
    return (
        <div className="tariff">
            <h2>Оптимальный тариф для вас</h2>
            <section className='tariff-optimal'>
                <div className="main-tariff">
                    <div key={mainTariff.id}>
                        <h3>{mainTariff.title}</h3>
                        <p className="tariff-price">
                            {mainTariff.price}₽ <span>в месяц</span>
                        </p>
                        <button
                            className="main-tariff-button"
                            onClick={() => handleTariffChange(mainTariff)}
                            disabled={isProcessing}
                        >
                            Выбрать тариф
                        </button>
                        {mainTariff.bundle && (
                            <ul className="tariff-features">
                                <li>{+mainTariff.bundle.internet / 1024} ГБ</li>
                                <li>{timeToMinutes(mainTariff.bundle.callTime)} Минут</li>
                                <li>{mainTariff.bundle.messages} SMS</li>
                            </ul>
                        )}
                    </div>
                </div>

                <div className="tariff-services">
                    <AdditionalServices cutValue={1} />
                </div>
            </section>
            <img className='img-plug' src='..\src\assets\img\plug.png' alt='пока просто заглушка'></img>

            <div className="tariff-cards">
                {slicedTariffsArray.map((tariff) => (
                    <div className="tariff-card-section" key={tariff.id}>
                        <h3 className="tariff-title">{tariff.title}</h3>
                        <p className="tariff-price">
                            {tariff.price}₽ <span>в месяц</span>
                        </p>
                        <button
                            className="tariff-button"
                            onClick={() => handleTariffChange(tariff)}
                            disabled={isProcessing}
                        >
                            Выбрать тариф
                        </button>
                        {tariff.bundle && (
                            <ul className="tariff-features">
                                <li>{ +tariff.bundle.internet /1024} ГБ</li>
                                <li>{timeToMinutes(tariff.bundle.callTime)} Минут</li>
                                <li>{tariff.bundle.messages} SMS</li>
                            </ul>
                        )}
                    </div>
                ))}
            </div>
        </div>
    );
};

export default Tariff;