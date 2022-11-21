import * as React from 'react';
import { Route } from 'react-router';
import Home from './components/Home';
import Ranking from './components/Ranking';
import { BrowserRouter, Switch } from 'react-router-dom';
import { StyledEngineProvider } from '@mui/material/styles';
import { ThemeProvider, createTheme } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";

const theme = createTheme({
    palette: { mode: "dark" },
    typography: {
        allVariants: {
            fontFamily: '"Montserrat", Open Sans'
        }
    }
});

export default class App extends React.Component {
    static displayName = App.name;

    render() {
        return (
            <ThemeProvider theme={theme}>
                <CssBaseline />
                <React.StrictMode>
                    <StyledEngineProvider injectFirst>
                        <BrowserRouter>
                            <Switch>
                                <Route exact path='/' component={Home} />
                                <Route path='/:id' component={Ranking} />
                            </Switch>
                        </BrowserRouter>
                    </StyledEngineProvider>
                </React.StrictMode>
            </ThemeProvider>
        );
    }
}
