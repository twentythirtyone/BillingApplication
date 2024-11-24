/* eslint-disable react/prop-types */
import { Doughnut } from "react-chartjs-2";
import { timeToMinutes, getMaxValue } from './functions.js'
import "chart.js/auto";

export const TariffOptions = ({ userData }) => {
    let currentTime = timeToMinutes(userData.callTime);
    let maxTime = getMaxValue(currentTime, timeToMinutes(userData.tariff.bundle.callTime));

    let currentInternet = userData.internet;
    let maxInternet = getMaxValue(currentInternet, userData.tariff.bundle.internet);

    let currentSMS = userData.messages;
    let maxSMS = getMaxValue(currentSMS, userData.tariff.bundle.messages);

    const dataOptions = [
        { label: "Минуты", value: currentTime, max: maxTime },
        { label: "Интернет", value: currentInternet, max: maxInternet, unit: "ГБ" },
        { label: "SMS", value: currentSMS, max: maxSMS },
    ];

    const generateChartData = (value, max) => ({
        datasets: [
            {
                data: [value, max - value],
                backgroundColor: ["#FF3B30", "#E0E0E0"],
                borderWidth: 0,
            },
        ],
    });

    return (
        <div className="tariff-options">
            {dataOptions.map((option, index) => (
                <div className="tariff-card" key={index}>
                    <p className='chart-name'>{option.label}</p>
                    <Doughnut
                        data={generateChartData(option.value, option.max)}
                        options={{
                            cutout: "92%",
                            plugins: {
                                tooltip: { enabled: false },
                                legend: { display: false },
                            },
                        }}
                        className="chart"
                    />
                    <div className="chart-value">
                        <div className='chart-value-top'>{option.value}{option.unit || ""}</div> из {option.max}{option.unit || ""}
                    </div>
                </div>
            ))}
        </div>
    );
}