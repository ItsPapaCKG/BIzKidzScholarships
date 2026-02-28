import { useEffect, useState } from "react";
import { BizDocumentType, type HTMLContentResponse } from "../../models/ViewModels";
import { APICall } from "../../services/APIService";

export interface HTMLContentProps {
    docType: BizDocumentType
}

export default function HtmlContent({ docType }: HTMLContentProps) {
    const [html, setHtml] = useState<string>("");
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    const fetchHtmlFromApi = async () => {
        let endpoint = "";

        switch (docType) {
            case BizDocumentType.TermsOfService:
                endpoint = "terms";
                break;
            case BizDocumentType.PrivacyPolicy:
                endpoint = "privacy";
                break;
            default:
                endpoint = "errorHTML";
                break
        }

        let res = await APICall<HTMLContentResponse>(endpoint, "GET", null, true);

        if (res.success) {
            return res!.data!.html;
        }

        return "<p class='text-danger'>An error has occurred.</p>"
    }

    useEffect(() => {
        const loadData = async () => {
            try {
                setLoading(true);

                const responseHtml = await fetchHtmlFromApi();

                setHtml(responseHtml);
            } catch (err) {
                setError("Failed to load content.");
            } finally {
                setLoading(false);
            }
        };

        loadData();
    }, []);

    if (loading) return <div>Loading...</div>;
    if (error) return <div>{error}</div>;

    return (
        <div
            dangerouslySetInnerHTML={{ __html: html }}
        />
    );
}