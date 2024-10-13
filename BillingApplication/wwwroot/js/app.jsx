class Hello extends React.Component {
    render() {
        return <button>Кнопка</button>;
    }
}
ReactDOM.render(
    <Hello />,
    document.getElementById("content")
);