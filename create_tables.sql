--
-- PostgreSQL database dump
--

\restrict NlS0w7kjhag8EUrdHhlr1LR7SvE0DQWKXrQVMplbiOUT2Nmt0BuUD0WPIFJxg0P

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

-- Started on 2026-11-03 14:28:51

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 5039 (class 0 OID 116402)
-- Dependencies: 220
-- Data for Name: izdeliya; Type: TABLE DATA; Schema: public; Owner: tcm_lab
--

INSERT INTO public.izdeliya VALUES (1, 'Дрель электрическая', 'Бытовая электродрель 650 Вт');
INSERT INTO public.izdeliya VALUES (2, 'Шуруповерт аккумуляторный', 'Дрель-шуруповерт 18 В');
INSERT INTO public.izdeliya VALUES (3, 'Перфоратор промышленный', 'Перфоратор SDS-Max 1200 Вт');


--
-- TOC entry 5043 (class 0 OID 116425)
-- Dependencies: 224
-- Data for Name: operatsii; Type: TABLE DATA; Schema: public; Owner: tcm_lab
--

INSERT INTO public.operatsii VALUES (1, 1, 1, 'Механическая обработка корпуса', 120, 'Механообработка');
INSERT INTO public.operatsii VALUES (2, 1, 2, 'Покраска деталей', 60, 'Покраска');
INSERT INTO public.operatsii VALUES (3, 1, 3, 'Узловая сборка', 90, 'Сборка');
INSERT INTO public.operatsii VALUES (4, 1, 4, 'Выходной контроль', 30, 'Тестирование');
INSERT INTO public.operatsii VALUES (5, 2, 1, 'Фрезеровка рукоятки', 90, 'Механообработка');
INSERT INTO public.operatsii VALUES (6, 2, 2, 'Монтаж платы и аккумулятора', 75, 'Сборка');
INSERT INTO public.operatsii VALUES (7, 2, 3, 'Тестирование батарейного блока', 45, 'Тестирование');
INSERT INTO public.operatsii VALUES (8, 3, 1, 'Расточка цилиндра', 150, 'Механообработка');
INSERT INTO public.operatsii VALUES (9, 3, 2, 'Нанесение антикоррозийного покрытия', 90, 'Покраска');
INSERT INTO public.operatsii VALUES (10, 3, 3, 'Сборка редуктора и двигателя', 120, 'Сборка');
INSERT INTO public.operatsii VALUES (11, 3, 4, 'Испытание под нагрузкой', 60, 'Тестирование');


--
-- TOC entry 5041 (class 0 OID 116413)
-- Dependencies: 222
-- Data for Name: resursy; Type: TABLE DATA; Schema: public; Owner: tcm_lab
--

INSERT INTO public.resursy VALUES (1, 'Станок токарный Т-100', 'Механообработка', 'Токарная обработка валов');
INSERT INTO public.resursy VALUES (2, 'Станок фрезерный F-200', 'Механообработка', 'Фрезеровка корпусов');
INSERT INTO public.resursy VALUES (3, 'Линия сборки Л-1', 'Сборка', 'Конвейерная сборка узлов');
INSERT INTO public.resursy VALUES (4, 'Стенд тестирования Т-1', 'Тестирование', 'Проверка электробезопасности');
INSERT INTO public.resursy VALUES (5, 'Покрасочная камера С-10', 'Покраска', 'Порошковая окраска');


--
-- TOC entry 5045 (class 0 OID 116445)
-- Dependencies: 226
-- Data for Name: zakazy; Type: TABLE DATA; Schema: public; Owner: tcm_lab
--

INSERT INTO public.zakazy VALUES (1, 1, 50, '2026-10-05 17:00:00', 'Новый');
INSERT INTO public.zakazy VALUES (2, 2, 100, '2026-10-02 12:00:00', 'Новый');
INSERT INTO public.zakazy VALUES (3, 3, 20, '2026-10-10 18:00:00', 'Новый');
INSERT INTO public.zakazy VALUES (4, 1, 30, '2026-10-01 15:00:00', 'Новый');
INSERT INTO public.zakazy VALUES (5, 2, 80, '2026-10-08 17:00:00', 'Новый');
INSERT INTO public.zakazy VALUES (6, 3, 15, '2026-10-12 12:00:00', 'Новый');
INSERT INTO public.zakazy VALUES (7, 1, 40, '2026-10-04 16:00:00', 'Новый');
INSERT INTO public.zakazy VALUES (8, 2, 60, '2026-10-06 14:00:00', 'Новый');
INSERT INTO public.zakazy VALUES (9, 3, 25, '2026-10-15 17:00:00', 'Новый');
INSERT INTO public.zakazy VALUES (10, 1, 70, '2026-10-03 18:00:00', 'Новый');


--
-- TOC entry 5047 (class 0 OID 116465)
-- Dependencies: 228
-- Data for Name: raspisaniye; Type: TABLE DATA; Schema: public; Owner: tcm_lab
--



--
-- TOC entry 5053 (class 0 OID 0)
-- Dependencies: 219
-- Name: izdeliya_id_izdeliya_seq; Type: SEQUENCE SET; Schema: public; Owner: tcm_lab
--

SELECT pg_catalog.setval('public.izdeliya_id_izdeliya_seq', 3, true);


--
-- TOC entry 5054 (class 0 OID 0)
-- Dependencies: 223
-- Name: operatsii_id_operatsii_seq; Type: SEQUENCE SET; Schema: public; Owner: tcm_lab
--

SELECT pg_catalog.setval('public.operatsii_id_operatsii_seq', 12, true);


--
-- TOC entry 5055 (class 0 OID 0)
-- Dependencies: 227
-- Name: raspisaniye_id_raspisaniya_seq; Type: SEQUENCE SET; Schema: public; Owner: tcm_lab
--

SELECT pg_catalog.setval('public.raspisaniye_id_raspisaniya_seq', 1, false);


--
-- TOC entry 5056 (class 0 OID 0)
-- Dependencies: 221
-- Name: resursy_id_resursa_seq; Type: SEQUENCE SET; Schema: public; Owner: tcm_lab
--

SELECT pg_catalog.setval('public.resursy_id_resursa_seq', 5, true);


--
-- TOC entry 5057 (class 0 OID 0)
-- Dependencies: 225
-- Name: zakazy_id_zakaza_seq; Type: SEQUENCE SET; Schema: public; Owner: tcm_lab
--

SELECT pg_catalog.setval('public.zakazy_id_zakaza_seq', 11, true);


-- Completed on 2026-11-03 14:28:52

--
-- PostgreSQL database dump complete
--

\unrestrict NlS0w7kjhag8EUrdHhlr1LR7SvE0DQWKXrQVMplbiOUT2Nmt0BuUD0WPIFJxg0P

