namespace MePoC.WordAssociation.Interfaces;
public interface IWordAssociationService
{
    Task InitializeChat(CancellationToken cancellationToken);

}
