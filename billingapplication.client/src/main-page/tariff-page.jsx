import { useEffect, useState } from "react";
import { getTariff, changeTariff, payTariff } from "../requests.jsx";
import ReactLoading from "react-loading";
import { getRandomObjects, timeToMinutes } from './functions.js';
import { AdditionalServices } from './additional-services.jsx';
import { TrafficChart } from './internet-trafic-graph.jsx';
import { CallsChart } from './calls-trafic-graph.jsx';
import axios from "axios";

export const TariffPage = () => {
  const [tariffs, setTariffs] = useState([{
    title: 'Загрузка...',
    price: 0,
  }]);
  const [mainTariff, setMainTariff] = useState(null);
  const [visibleIndex, setVisibleIndex] = useState(0);
  const [isProcessing, setIsProcessing] = useState(false);
  const [userTariffId, setUserTariffId] = useState();

  useEffect(() => {
    document.title='Тариф'

    const token = localStorage.getItem("token");

    const fetchUserData = async () => {
      try {
          const response = await axios.get("/billingapplication/subscribers/current", {
              headers: {
                  Authorization: `Bearer ${token}`,
                  Accept: "*/*",
              },
          });

          setUserTariffId(response.data.tariff.id);
      } catch (error) {
          console.error("Ошибка при получении данных пользователя:", error);
      }
  };


    const fetchTariff = async (token) => {
      const tarif = await getTariff(token);
      setTariffs(tarif);

      // Устанавливаем главный тариф только при загрузке данных
      if (tarif.length > 0) {
        const filteredTariffs = tarif.filter(
          (t) => t.title !== "Стандартный"
      );
        const randomTariff = getRandomObjects(filteredTariffs, 1)[0];
        setMainTariff(randomTariff);
      }
    };
    fetchUserData();
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
    
        // Выполнение операций
        await changeTariff(tariff.id);
        await payTariff(tariff.id);
    
        alert("Тариф успешно изменен и оплачен!");
      } catch {
        alert("На вашем счету недостаточно средств");
      } finally {
        setIsProcessing(false);
      }
    }
  };

  const filteredTariffs = tariffs.filter(
    (t) => t.id !== userTariffId);

  const handleNext = () => {
    setVisibleIndex((prev) => (prev + 3) % filteredTariffs.length);
  };

  const handlePrev = () => {
    setVisibleIndex((prev) => (prev - 3 + filteredTariffs.length) % filteredTariffs.length);
  };

  if (!tariffs.length || !mainTariff || !userTariffId) {
    return (
      <div className="tariff">
        <ReactLoading
          type="cylon"
          color="#FF3B30"
          height={667}
          width={375}
          className="loading"
        />
      </div>
    );
  }

  const visibleTariffs = filteredTariffs.slice(visibleIndex, visibleIndex + 3);

  return (
    <div className="tariff">
      <h2>Оптимальный тариф для вас</h2>
      <section className="tariff-optimal">
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
      <div className="tariff-graph-sect">
        <TrafficChart />
        <CallsChart />
      </div>
      <div className="tariff-cards-container">
  {visibleIndex > 0 && (
    <button className="prev-page tariff-left" onClick={handlePrev}>
      ‹
    </button>
  )}
  <div className="tariff-cards">
            {visibleTariffs.map((tariff) => (
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
                    <li>{+tariff.bundle.internet / 1024} ГБ</li>
                    <li>{timeToMinutes(tariff.bundle.callTime)} Минут</li>
                    <li>{tariff.bundle.messages} SMS</li>
                </ul>
                )}
            </div>
            ))}
        </div>
        {visibleIndex + 3 < filteredTariffs.length && (
            <button className="next-page tariff-right" onClick={handleNext}>
            ›
            </button>
        )}
        </div>
    </div>
  );
};